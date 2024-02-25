var DetectMobile = {
	IsMobile: function()
	{
		return Module.SystemInfo.mobile;
	}
};

mergeInto(LibraryManager.library, DetectMobile);