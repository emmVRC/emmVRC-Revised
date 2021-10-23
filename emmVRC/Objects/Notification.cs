
using System;
using UnityEngine;

namespace emmVRC.Objects
{
    public class Notification
    {
        public readonly DateTime timeCreated = DateTime.Now;
        public readonly string name = "";
        public readonly Sprite icon = null;
        public readonly string content = "";
        public readonly bool canIgnore = true;
        public readonly bool showAcceptButton = true;
        public readonly Action acceptButton = null;
        public readonly string acceptButtonText = "";
        public readonly string acceptButtonTooltip = "";
        public readonly bool showIgnoreButton = true;
        public readonly Action ignoreButton = null;
        public readonly string ignoreButtonText = "";
        public readonly string ignoreButtonTooltip = "";

        public Notification(string name = "", Sprite icon = null, string content = "", bool canIgnore = true, bool showAcceptButton = true, Action acceptButton = null, string acceptButtonText = "", string acceptButtonTooltip = "", bool showIgnoreButton = true, Action ignoreButton = null, string ignoreButtonText = "", string ignoreButtonTooltip = "")
        {
            this.name = name;
            this.icon = icon;
            this.content = content;
            this.canIgnore = canIgnore;
            this.showAcceptButton = showAcceptButton;
            this.acceptButton = acceptButton;
            this.acceptButtonText = acceptButtonText;
            this.acceptButtonTooltip = acceptButtonTooltip;
            this.showIgnoreButton = showIgnoreButton;
            this.ignoreButton = ignoreButton;
            this.ignoreButtonText = ignoreButtonText;
            this.ignoreButtonTooltip = ignoreButtonTooltip;
        }
    }
}