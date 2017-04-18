﻿using System;
using System.IO;

namespace GitCommands
{
    public class CommitDto
    {
        public CommitDto(string message, bool amend)
        {
            Message = message;
            Amend = amend;
        }

        public string Message { get; set; }
        public string Result { get; set; }
        public bool Amend { get; set; }
    }

    public class CommitHelper
    {
        public CommitHelper(CommitDto dto)
        {
            Dto = dto;
        }

        public CommitDto Dto { get; set; }

        public void Execute(GitModule module)
        {
            if (Dto.Amend)
                Dto.Result = module.RunGitCmd("commit --amend -m \"" + Dto.Message + "\"");
            else
                Dto.Result = module.RunGitCmd("commit -m \"" + Dto.Message + "\"");
        }

        public static void SetCommitMessage(GitModule module, string commitMessageText, bool amendCommit)
        {
            if (string.IsNullOrEmpty(commitMessageText))
            {
                File.Delete(GetCommitMessagePath(module));
                File.Delete(GetAmendPath(module));
                return;
            }

            using (var textWriter = new StreamWriter(GetCommitMessagePath(module), false, module.CommitEncoding))
            {
                textWriter.Write(commitMessageText);
            }
            if(AppSettings.RememberAmendCommitState && amendCommit)
                File.WriteAllText(GetAmendPath(module), true.ToString());
            else if(File.Exists(GetAmendPath(module)))
                File.Delete(GetAmendPath(module));
        }

        public static string GetCommitMessagePath(GitModule module)
        {
            return GetFilePath(module, "COMMITMESSAGE");
        }

        public static string GetAmendPath(GitModule module)
        {
            return GetFilePath(module, "GitExtensions.amend");
        }

        public static string GetFilePath(GitModule module, string action)
        {
            return Path.Combine(module.GetGitDirectory(), action);
        }
    }
}