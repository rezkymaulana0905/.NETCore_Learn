// Show the modal on page load if the user hasn't accepted the privacy policy yet
window.onload = function () {
    if (!getCookie("privacyPolicyAccepted")) {
        document.getElementById("privacyPolicyModal").style.display = "block";
    }
};

// Close the modal when the user clicks the 'Accept' button
document.getElementById("acceptPrivacyPolicy").onclick = function () {
    setCookie("privacyPolicyAccepted", "true", 365);
    document.getElementById("privacyPolicyModal").style.display = "none";
};

// Close the modal when the user clicks the 'X' button
document.getElementsByClassName("close")[0].onclick = function () {
    document.getElementById("privacyPolicyModal").style.display = "none";
};

// Utility function to set a cookie
function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

// Utility function to get a cookie
function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}