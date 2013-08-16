﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceControl.Assemblys
{

    public class AssemblySource
    {
        public string SourceUriOrigin { get; private set; }// repository url
        public string ProjectFilePath { get; private set; }
        public string Name { get; set; }
        private bool IsUpToDate { get; set; }
        public string lastBuildLog { get; private set; }

        public bool IsActualToRemote
        {
            get
            {
                if (!IsUpToDate)
                    return SetUpToDate();
                return true;
            }
        }
        public SCM scm;

        public AssemblySource(string root, string projFileRelativePath, string remoteUri)
        {
            SourceUriOrigin = remoteUri;
            Name = System.IO.Path.GetFileNameWithoutExtension(projFileRelativePath);
            ProjectFilePath = System.IO.Path.Combine(root, Name, projFileRelativePath);
            string dir = System.IO.Path.Combine(root, Name);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            Type T = typeof(Git.git_scm);
            scm = (SCM)Activator.CreateInstance(T, new object[] { dir, remoteUri });
        }

        public bool BuildProject(out byte[] library, out byte[] symbols)
        {
            library = symbols = null;
            AssemblyBuilder builder = new AssemblyBuilder(ProjectFilePath);
            if (builder.BuildProject())
            {
                library = File.ReadAllBytes(builder.BuildResultDll);
                symbols = File.ReadAllBytes(builder.BuildResultSymbols);
                lastBuildLog = builder.Log;
                return true;
            }
            else
            {
                lastBuildLog = builder.Log;
                Console.WriteLine("build project failure: {0}", builder.Log);
            }
            return false;
        }
        public bool SetUpToDate()
        {
            return IsUpToDate = lSetUpToDate();
        }
        private bool lSetUpToDate()
        {
            switch (scm.CurrentStatus)
            {
                case SCM.Status.none:
                    scm.UpdateStatus();
                    if (scm.CurrentStatus != SCM.Status.none)
                        return lSetUpToDate();
                    return false;
                // 
                case SCM.Status.cloneFailure:
                    return scm.Clone();
                case SCM.Status.cloneRequired:
                    return scm.Clone();
                //
                case SCM.Status.fetchFailure:
                    return scm.Fetch();

                case SCM.Status.fetchRequied:
                    return scm.Fetch();

                case SCM.Status.allUpToDate:
                    return true;
                default:
                    break;
            }
            return false;
        }
    }
}