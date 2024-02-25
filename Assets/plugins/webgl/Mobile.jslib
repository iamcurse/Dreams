var Mobile = {
    IsMobile: function()
     {
         return UnityLoader.SystemInfo.mobile;
     }
};

mergeInto(LibraryManager.library, Mobile);