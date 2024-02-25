function requestFullScreen() {
    var element = document.documentElement;
    var requestMethod = element.requestFullscreen || element.webkitRequestFullscreen || element.mozRequestFullScreen || element.msRequestFullscreen;

    if (requestMethod) { // Native full screen.
        requestMethod.call(element);
    }
}

// Auto fullscreen on mobile devices
function autoFullscreenOnMobile() {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        requestFullScreen();
    }
}

// Call autoFullscreenOnMobile function when the page loads
window.onload = function() {
    autoFullscreenOnMobile();
};
