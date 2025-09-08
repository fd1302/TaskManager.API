// Login
function login(event) {
    event.preventDefault();
    const loginInfo = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value,
        role: document.getElementById("role").value
    };
    checkInputValues(loginInfo);
    if(loginInfo.role === "Tenant") {
        tenantLogin(loginInfo.userName, loginInfo.password);
    } else if(loginInfo.role === "Admin") {
        adminLogin(loginInfo.userName, loginInfo.password);
    } else if(loginInfo.role === "Manager") {
        managerLogin(loginInfo.userName, loginInfo.password);
    } else if(loginInfo.role === "Member") {
        memberLogin(loginInfo.userName, loginInfo.password);
    } else {
        return;
    }
}

function checkInputValues(loginInfo) {
    let userNameLabel = document.getElementById("userNameLabel");
    let passwordLabel = document.getElementById("passwordLabel");
    let roleLabel = document.getElementById("roleLabel");
    if(loginInfo.userName === "") {
        userNameLabel.textContent = "Username*";
        userNameLabel.style.color = "#ff6363ff";
    }
    if(loginInfo.password === "") {
        passwordLabel.textContent = "Username*";
        passwordLabel.style.color = "#ff6363ff";
    }
    if(loginInfo.role === "") {
        roleLabel.textContent = "Role - Select your role please";
        roleLabel.style.color = "#ff6363ff";
    }
}

function loginErrorMessages(message) {
    let userNameError = document.getElementById("userNameError");
    let passwordError = document.getElementById("passwordError");
    if(message === "User not found.") {
        userNameError.textContent = "Wrong username.";
        passwordError.textContent = "";
    }
    if(message === "Wrong password.") {
        passwordError.textContent = "Wrong password, try again.";
        userNameError.textContent = "";
    }
}

async function tenantLogin(userName, password) {
    const tenantLoginDto = {
        userName,
        password
    }
    try {
        const response = await fetch(`${authUrl}/tenantlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(tenantLoginDto)
        });
        const result = await response.json();
        if(response.ok) {
            window.location.href = "profile.html"
        } else if(!response.ok) {
            loginErrorMessages(result.errorMessage);
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function adminLogin(userName, password) {
    const adminLoginDto = {
        userName,
        password
    }
    try {
        const response = await fetch(`${authUrl}/adminlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(adminLoginDto)
        });
        if(response.ok) {
            window.location.href = "profile.html";
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function managerLogin(userName, password) {
    const managerLoginDto = {
        userName,
        password
    }
    try {
        const response = await fetch(`${authUrl}/managerlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(managerLoginDto)
        });
        if(response.ok) {
            window.location.href = "profile.html";
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function memberLogin(userName, password) {
    const memberLoginDto = {
        userName,
        password
    }
    try {
        const response = await fetch(`${authUrl}/memberlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(memberLoginDto)
        });
        if(response.ok) {
            window.location.href = "profile.html";
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

// Signup
function tenantSignupForm() {
    const toggleLink = document.getElementById("toggleForm");
    const formTitle = document.getElementById("formTitle");
    const loginBtn = document.getElementById("loginBtn");
    const signupBtn = document.getElementById("signupBtn");
    const signupFields = document.querySelectorAll(".signup-only");
    const role = document.getElementById("roleDiv");

    if (formTitle.textContent === "Sign Up As Tenant") {
        formTitle.textContent = "Login";
        loginBtn.style.display = "block"
        signupBtn.style.display = "none";
        toggleLink.textContent = "No account? Sign up";
        role.style.display = "block";

        signupFields.forEach(field => field.style.display = "none");
    } else {
        formTitle.textContent = "Sign Up As Tenant";
        loginBtn.style.display = "none"
        signupBtn.style.display = "block";
        toggleLink.textContent = "Have account? Login";
        role.style.display = "none";

        signupFields.forEach(field => field.style.display = "block");
    }
}

// Logout
async function logout() {
    await fetch(`${authUrl}/logout`, {
        method: "POST",
        credentials: "include"
    });
    window.location.reload(true);
    window.location.href = "login.html";
}