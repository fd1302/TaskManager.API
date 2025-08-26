const loginUrl = "/api/authentication"

// Login
function login(event) {
    event.preventDefault();
    const loginInfo = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value,
        role: document.getElementById("role").value
    }
    if(loginInfo.role === "Tenant") {
        tenantLogin(loginInfo.userName, loginInfo.password);
    }
    else if(loginInfo.role === "Admin") {
        adminLogin(loginInfo.userName, loginInfo.password);
    }
    else if(loginInfo.role === "Manager") {
        managerLogin(loginInfo.userName, loginInfo.password);
    }
    else if(loginInfo.role === "Member") {
        memberLogin(loginInfo.userName, loginInfo.password);
    } else {
        return;
    }
}

async function tenantLogin(userName, password) {
    const tenantLoginDto = {
        userName,
        password
    }
    try {
        const response = await fetch(`${loginUrl}/tenantlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(tenantLoginDto)
        });
        if(response.ok) {
            window.location.href = "profile.html"
        }
        else if(!response.ok) {
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
        const response = await fetch(`${loginUrl}/adminlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(adminLoginDto)
        });
        if(response.ok) {
            window.location.href = "profile.html";
        }
        else if(!response.ok) {
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
        const response = await fetch(`${loginUrl}/managerlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(managerLoginDto)
        });
        if(response.ok) {
            window.location.href = "profile.html";
        }
        else if(!response.ok) {
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
        const response = await fetch(`${loginUrl}/memberlogin`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                credentials: "include"
            },
            body: JSON.stringify(memberLoginDto)
        });
        if(response.ok) {
            window.location.href = "profile.html";
        }
        else if(!response.ok) {
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
    }
    else {
        formTitle.textContent = "Sign Up As Tenant";
        loginBtn.style.display = "none"
        signupBtn.style.display = "block";
        toggleLink.textContent = "Have account? Login";
        role.style.display = "none";

        signupFields.forEach(field => field.style.display = "block");
    }
}