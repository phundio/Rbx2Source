﻿#pragma warning disable 0649
using System.Collections.Generic;

namespace Rbx2Source.Web
{
    class Rbx3DThumbnailInfo
    {
        public string Obj;
        public string Mtl;
        public string[] Textures;
    }

    class TextureFetch
    {
        public static Rbx3DThumbnailInfo Get3DThumbnail(int userId)
        {
            Rbx3DThumbnailInfo info = null;
            string url = RbxWebUtility.PendCdnUrl("http://www.roblox.com/avatar-thumbnail-3d/json?userId=" + userId);
            if (url != null)
                info = RbxWebUtility.DownloadJSON<Rbx3DThumbnailInfo>(url);

            return info;
        }

        public static List<string> FromUser(int userId)
        {
            List<string> result = new List<string>();
            Rbx3DThumbnailInfo info = Get3DThumbnail(userId);
            if (info != null)
                foreach (string textureHash in info.Textures)
                    result.Add(RbxWebUtility.ResolveHashUrl(textureHash));

            return result;
        }

    }
}
