var MobileDetect = {
    IsMobile: function()
    {
        return Mobile.SystemInfo.mobile;
    }
};

mergeInto(LibraryManager.Library, MobileDetect);