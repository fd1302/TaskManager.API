function checkSignupInputValues(informations) {
    let userNameLabel = document.getElementById("userNameLabel");
    let passwordLabel = document.getElementById("passwordLabel");
    let emailLabel = document.getElementById("emailLabel");
    if(informations.userName === "") {
        userNameLabel.textContent = "Username*";
        userNameLabel.style.color = "#ff6363ff";
    }
    if(informations.password === "") {
        passwordLabel.textContent = "Password*";
        passwordLabel.style.color = "#ff6363ff";
    }
    if(informations.email === "") {
        emailLabel.textContent = "Email*";
        emailLabel.style.color = "#ff6363ff";
    }
}