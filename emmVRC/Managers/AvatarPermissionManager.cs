using emmVRC.Objects;
using emmVRC.TinyJSON;
using System.Collections.Generic;
using System.IO;

namespace emmVRC.Managers
{
    public class AvatarPermissionsManager
    {
        public static readonly string AvatarPermissionsFolder = Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/AvatarPermissions");

        private static readonly Dictionary<string, CachedValue<AvatarPermissions>> cache = new Dictionary<string, CachedValue<AvatarPermissions>>();

        public static bool TryGetAvatarPermissions(string avatarId, out AvatarPermissions permissions)
        {
            if (cache.TryGetValue(avatarId, out CachedValue<AvatarPermissions> cachedPerms))
            {
                if (cachedPerms.Validate())
                {
                    permissions = cachedPerms.value;
                    return true;
                }
                else
                {
                    cache.Remove(avatarId);
                }
            }

            if (!Directory.Exists(AvatarPermissionsFolder))
                Directory.CreateDirectory(AvatarPermissionsFolder);

            if (!File.Exists(Path.Combine(AvatarPermissionsFolder, avatarId)))
            {
                permissions = null;
                return false;
            }
            string fileText = File.ReadAllText(Path.Combine(AvatarPermissionsFolder, avatarId));
            if (fileText.Contains("emmVRC.Managers.AvatarPermissions")) {
                fileText.Replace("emmVRC.Managers.AvatarPermissions", "emmVRC.Objects.AvatarPermissions");
                File.WriteAllText(Path.Combine(AvatarPermissionsFolder, avatarId), fileText);
            }
            permissions = Decoder.Decode(fileText).Make<AvatarPermissions>();
            cache.Add(avatarId, permissions);
            return true;
        }

        public static void SaveAvatarPermissions(AvatarPermissions avatarPermission)
        {
            if (!Directory.Exists(AvatarPermissionsFolder))
                Directory.CreateDirectory(AvatarPermissionsFolder);

            if (string.IsNullOrEmpty(avatarPermission.AvatarId))
                return;

            if (cache.ContainsKey(avatarPermission.AvatarId))
                cache[avatarPermission.AvatarId] = avatarPermission;
            else
                cache.Add(avatarPermission.AvatarId, avatarPermission);

            File.WriteAllText(Path.Combine(AvatarPermissionsFolder, avatarPermission.AvatarId), Encoder.Encode(avatarPermission, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints));
        }
    }
}