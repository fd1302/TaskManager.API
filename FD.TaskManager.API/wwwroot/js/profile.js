async function getUserInfoFromToken() {
    try {
        const response = await fetch(`${authUrl}/senduserinfo`);
        if (!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        const info = await response.json();
        return info;
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function checkUserOptions() {
    const info = await getUserInfoFromToken();
    const addProject = document.getElementById("addProject");
    const projects = document.getElementById("projects");
    const tasks = document.getElementById("tasks");
    const hire = document.getElementById("hireMember");
    const editProfile = document.getElementById("editProfile");
    const showAdmins = document.getElementById("showAdmins");
    const showManagers = document.getElementById("showManagers");
    const showMembers = document.getElementById("showMembers");
    if(info.role === "Tenant") {
        editProfile.onclick = function() { tenantUpdateStyle() };
        tasks.style.display = "none";
    } else if(info.role === "Admin") {
        editProfile.onclick = function() { adminUpdateStyle() };
        showAdmins.style.display = "none";
        tasks.style.display = "none";
    } else if(info.role === "Manager") {
        editProfile.onclick = function() { managerUpdateStyle() };
        addProject.style.display = "none";
        showAdmins.style.display = "none";
        showManagers.style.display = "none";
        tasks.style.display = "none";
        hire.style.display = "none";
    } else if(info.role === "Member") {
        editProfile.onclick = function() { memberUpdateStyle() };
        addProject.style.display = "none";
        projects.style.display = "none";
        hire.style.display = "none";
        showAdmins.style.display = "none";
        showManagers.style.display = "none";
        showMembers.style.display = "none";
    }
}
checkUserOptions();

async function infoStyle() {
    const userInfo = await getUserInfoFromToken();
    if(userInfo.role === "Tenant") {
        const tenant = await getTenant();
        tenantInfoStyle(tenant);
    }
    else if(userInfo.role === "Admin") {
        const admin = await getAdmin();
        adminInfoStyle(admin);
    }
    else if(userInfo.role === "Manager") {
        const manager = await getManager();
        managerInfoStyle(manager);
    }
    else if(userInfo.role === "Member") {
        const member = await getMember();
        memberInfoStyle(member);
    }
}
infoStyle();

// Tenant functions
async function getTenant() {
    const userInfo = await getUserInfoFromToken();
    if(userInfo.role === "Tenant") {
        try {
            const response = await fetch(`${tenantUrl}/gettenant`);
            if(!response.ok) {
                throw new Error(`There was a problem while handling the request: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }
}

function tenantInfoStyle(info) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-userinfo";
    const tenantDiv = document.createElement("div");
    tenantDiv.classList.add("user-info");
    mainContainer.appendChild(tenantDiv);
    const itemTitle = document.createElement("h2");
    itemTitle.textContent = "(Tenant)";
    tenantDiv.appendChild(itemTitle);
    const nameLabel = document.createElement("label");
    nameLabel.textContent = "Name:";
    tenantDiv.appendChild(nameLabel);
    const name = document.createElement("h3");
    name.textContent = info.name;
    tenantDiv.appendChild(name);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description:";
    tenantDiv.appendChild(descriptionLabel);
    const description = document.createElement("h3");
    description.textContent = info.description;
    tenantDiv.appendChild(description);
    const createdAtLabel = document.createElement("label");
    createdAtLabel.textContent = "Tenant Created At:";
    tenantDiv.appendChild(createdAtLabel);
    const createdAt = document.createElement("h3");
    createdAt.textContent = info.createdAt;
    tenantDiv.appendChild(createdAt);
}

async function updateTenant() {
    const updateDto = {
        name: document.getElementById("nameUpdate").value,
        description: document.getElementById("descriptionUpdate").value
    };
    try {
        const response = await fetch(`${tenantUrl}/update`, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        });
        const jsonResponse = await response.json();
        if(response.ok) {
            alert(jsonResponse.message);
        }
        else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${jsonResponse}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function tenantUpdateStyle() {
    const tenant = await getTenant();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item";
    const tenantUpdate = document.createElement("div");
    tenantUpdate.classList.add("add-update-container");
    tenantUpdate.id = "tenantUpdate";
    mainContainer.appendChild(tenantUpdate);
    const nameLabel = document.createElement("label");
    nameLabel.textContent = "Name: ";
    tenantUpdate.appendChild(nameLabel);
    const nameInput = document.createElement("input");
    nameInput.type = "text";
    nameInput.id = "nameUpdate";
    nameInput.value = tenant.name;
    tenantUpdate.appendChild(nameInput);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    tenantUpdate.appendChild(descriptionLabel);
    const description = document.createElement("input");
    description.type = "text";
    description.id = "descriptionUpdate";
    description.value = tenant.description;
    tenantUpdate.appendChild(description);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Update";
    updateBtn.onclick = function() { updateTenant() };
    tenantUpdate.appendChild(updateBtn);
}



// Admin functions
async function getAdmin() {
    try {
        const response = await fetch(`${adminUrl}/getadmin`);
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

function adminInfoStyle(info) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-userinfo";
    const adminDiv = document.createElement("div");
    adminDiv.classList.add("user-info");
    mainContainer.appendChild(adminDiv);
    const itemTitle = document.createElement("h2");
    itemTitle.textContent = "(Admin)";
    adminDiv.appendChild(itemTitle);
    const userNameLabel = document.createElement("label");
    userNameLabel.textContent = "Username:";
    adminDiv.appendChild(userNameLabel);
    const userName = document.createElement("h3");
    userName.textContent = info.userName;
    adminDiv.appendChild(userName);
    const emailLabel = document.createElement("label");
    emailLabel.textContent = "Email:";
    adminDiv.appendChild(emailLabel);
    const email = document.createElement("h3");
    email.textContent = info.email;
    adminDiv.appendChild(email);
    const tenantNameLabel = document.createElement("label");
    tenantNameLabel.textContent = "Tenant:";
    adminDiv.appendChild(tenantNameLabel);
    const tenantName = document.createElement("h3");
    tenantName.textContent = info.tenantName;
    adminDiv.appendChild(tenantName);
    const joinedAtLabel = document.createElement("label");
    joinedAtLabel.textContent = "Joined at:";
    adminDiv.appendChild(joinedAtLabel);
    const joinedAt = document.createElement("h3");
    joinedAt.textContent = info.joinedAt;
    adminDiv.appendChild(joinedAt);
}

async function showAdmins() {
    const admins = await getAdminsWithTenantId();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    admins.forEach(admin => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2");
        itemTitle.textContent = "(Admin)";
        item.appendChild(itemTitle);
        const userNameLabel = document.createElement("label");
        userNameLabel.textContent = "Username: ";
        item.appendChild(userNameLabel);
        const userName = document.createElement("h3");
        userName.textContent = admin.userName;
        item.appendChild(userName);
        const emailLabel = document.createElement("label");
        emailLabel.textContent = "Email: ";
        item.appendChild(emailLabel);
        const email = document.createElement("h3");
        email.textContent = admin.email;
        item.appendChild(email);
        const joinedAtLabel = document.createElement("label");
        joinedAtLabel.textContent = "Joined at: ";
        item.appendChild(joinedAtLabel);
        const joinedAt = document.createElement("h3");
        joinedAt.textContent = admin.joinedAt;
        item.appendChild(joinedAt);
        const demotionBtn = document.createElement("button");
        demotionBtn.textContent = "Demote to manager";
        demotionBtn.onclick = function() { demoteToManager(admin.id) };
        item.appendChild(demotionBtn);
    })
}

async function adminUpdateStyle() {
    const admin = await getAdmin();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item";
    const updateDiv = document.createElement("div");
    updateDiv.classList.add("add-update-container");
    mainContainer.appendChild(updateDiv);
    const userNameLabel = document.createElement("label");
    userNameLabel.textContent = "Username: ";
    updateDiv.appendChild(userNameLabel);
    const userName = document.createElement("input");
    userName.type = "text";
    userName.value = admin.userName;
    userName.id = "adminUserName";
    updateDiv.appendChild(userName)
    const emailLabel = document.createElement("label");
    emailLabel.textContent = "Email: ";
    updateDiv.appendChild(emailLabel);
    const adminEmail = document.createElement("input");
    adminEmail.type = "text";
    adminEmail.value = admin.email;
    adminEmail.id = "adminEmail";
    updateDiv.appendChild(adminEmail);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Update";
    updateBtn.onclick = function() { updateAdmin() };
    updateDiv.appendChild(updateBtn);
}

// Manager functions
async function getManager() {
    try {
        const response = await fetch(`${managerUrl}/getmanager`);
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function managerInfoStyle(info) {
    const tenant = await getTenant();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-userinfo";
    const managerDiv = document.createElement("div");
    managerDiv.classList.add("user-info");
    mainContainer.appendChild(managerDiv);
    const itemTitle = document.createElement("h2");
    itemTitle.textContent = "(Manager)";
    managerDiv.appendChild(itemTitle);
    const userNameLabel = document.createElement("label");
    userNameLabel.textContent = "Username:";
    managerDiv.appendChild(userNameLabel);
    const userName = document.createElement("h3");
    userName.textContent = info.userName;
    managerDiv.appendChild(userName);
    const emailLabel = document.createElement("label");
    emailLabel.textContent = "Email:";
    managerDiv.appendChild(emailLabel);
    const email = document.createElement("h3");
    email.textContent = info.email;
    managerDiv.appendChild(email);
    const tenantNameLabel = document.createElement("label");
    tenantNameLabel.textContent = "Tenant:";
    managerDiv.appendChild(tenantNameLabel);
    const tenantName = document.createElement("h3");
    tenantName.textContent = info.tenantName;
    managerDiv.appendChild(tenantName);
    const joinedAtLabel = document.createElement("label");
    joinedAtLabel.textContent = "Joined at:";
    managerDiv.appendChild(joinedAtLabel);
    const joinedAt = document.createElement("h3");
    joinedAt.textContent = info.joinedAt;
    managerDiv.appendChild(joinedAt);
}

async function showManagers() {
    const managers = await getManagersWithTenantId();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    const emptyManager = jsonIsEmpty(managers);
    if(emptyManager == 1) {
        const emptyText = document.createElement("h2");
        emptyText.textContent = "No manager found.";
        mainContainer.appendChild(emptyText);
    }
    managers.forEach(manager => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2");
        itemTitle.textContent = "(Manager)";
        item.appendChild(itemTitle);
        const userNameLabel = document.createElement("label");
        userNameLabel.textContent = "Username: ";
        item.appendChild(userNameLabel);
        const userName = document.createElement("h3");
        userName.textContent = manager.userName;
        item.appendChild(userName);
        const emailLabel = document.createElement("label");
        emailLabel.textContent = "Email: ";
        item.appendChild(emailLabel);
        const email = document.createElement("h3");
        email.textContent = manager.email;
        item.appendChild(email);
        const joinedAtLabel = document.createElement("label");
        joinedAtLabel.textContent = "Joined at: ";
        item.appendChild(joinedAtLabel);
        const joinedAt = document.createElement("h3");
        joinedAt.textContent = manager.joinedAt;
        item.appendChild(joinedAt);
        const buttonsDiv = document.createElement("div");
        buttonsDiv.classList.add("item-buttons");
        item.appendChild(buttonsDiv);
        const buttonsSpan = document.createElement("span");
        buttonsDiv.appendChild(buttonsSpan);
        const promotionBtn = document.createElement("a");
        promotionBtn.textContent = "Promotion ";
        promotionBtn.onclick = function() { promoteToAdmin(manager.id) };
        buttonsSpan.appendChild(promotionBtn);
        const separator = document.createElement("a");
        separator.textContent = " / ";
        buttonsSpan.appendChild(separator);
        const demotionBtn = document.createElement("a");
        demotionBtn.textContent = "Demotion";
        demotionBtn.onclick = function() { demoteToMember(manager.id) };
        buttonsSpan.appendChild(demotionBtn);
    });
}

async function managerUpdateStyle() {
    const manager = await getManager();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item";
    const updateDiv = document.createElement("div");
    updateDiv.classList.add("add-update-container");
    mainContainer.appendChild(updateDiv);
    const userNameLabel = document.createElement("label");
    userNameLabel.textContent = "Username: ";
    updateDiv.appendChild(userNameLabel);
    const userName = document.createElement("input");
    userName.type = "text";
    userName.value = manager.userName;
    userName.id = "managerUserName";
    updateDiv.appendChild(userName)
    const emailLabel = document.createElement("label");
    emailLabel.textContent = "Email: ";
    updateDiv.appendChild(emailLabel);
    const adminEmail = document.createElement("input");
    adminEmail.type = "text";
    adminEmail.value = manager.email;
    adminEmail.id = "managerEmail";
    updateDiv.appendChild(adminEmail);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Update";
    updateBtn.onclick = function() { updateManager() };
    updateDiv.appendChild(updateBtn);
}

// Member function
async function getMember() {
    try {
        const response = await fetch(`${memberUrl}/getmember`);
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function getMembersWithNoTenant() {
    try {
        const response = await fetch(`${memberUrl}/getmemberswithnotenant`);
        if(!response.ok) {
            throw new Error(`There was a problem while fetching data: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function addMemberToTenant(id) {
    try {
        const response = await fetch(`${memberUrl}/jointenant?memberId=${id}`, {
            method: "PATCH"
        })
        if(response.ok) {
            showMembersWithNoTenant();
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

function memberInfoStyle(info) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.style.display = "flex";
    mainContainer.style.justifyContent = "center";
    const memberDiv = document.createElement("div");
    memberDiv.classList.add("user-info");
    mainContainer.appendChild(memberDiv);
    const itemTitle = document.createElement("h2");
    itemTitle.textContent = "(Member)";
    memberDiv.appendChild(itemTitle);
    const userNameLabel = document.createElement("label");
    userNameLabel.textContent = "Username:";
    memberDiv.appendChild(userNameLabel);
    const userName = document.createElement("h3");
    userName.textContent = info.userName;
    memberDiv.appendChild(userName);
    const emailLabel = document.createElement("label");
    emailLabel.textContent = "Email:";
    memberDiv.appendChild(emailLabel);
    const email = document.createElement("h3");
    email.textContent = info.email;
    memberDiv.appendChild(email);
    if(info.tenantName != null) {
        const tenantNameLabel = document.createElement("label");
        tenantNameLabel.textContent = "Tenant:";
        memberDiv.appendChild(tenantNameLabel);
        const tenantName = document.createElement("h3");
        tenantName.textContent = info.tenantName;
        memberDiv.appendChild(tenantName);
    }
    const joinedAtLabel = document.createElement("label");
    joinedAtLabel.textContent = "Joined at:";
    memberDiv.appendChild(joinedAtLabel);
    const joinedAt = document.createElement("h3");
    joinedAt.textContent = info.joinedAt;
    memberDiv.appendChild(joinedAt);
}

async function tenantMembers() {
    const members = await getMembersWithTenantId();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    const emptyMember = jsonIsEmpty(members);
    if(emptyMember == 1) {
        const emptyText = document.createElement("h2");
        emptyText.textContent = "No member found.";
        mainContainer.appendChild(emptyText);
    }
    members.forEach(member => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2");
        itemTitle.textContent = "(Member)";
        item.appendChild(itemTitle);
        const userNameLabel = document.createElement("label");
        userNameLabel.textContent = "Username: ";
        item.appendChild(userNameLabel);
        const userName = document.createElement("h3");
        userName.textContent = member.userName;
        item.appendChild(userName);
        const emailLabel = document.createElement("label");
        emailLabel.textContent = "Email: ";
        item.appendChild(emailLabel);
        const email = document.createElement("h3");
        email.textContent = member.email;
        item.appendChild(email);
        const joinedAtLabel = document.createElement("label");
        joinedAtLabel.textContent = "Joined at: ";
        item.appendChild(joinedAtLabel);
        const joinedAt = document.createElement("h3");
        joinedAt.textContent = member.joinedAt;
        item.appendChild(joinedAt);
        const buttonsDiv = document.createElement("div");
        buttonsDiv.classList.add("item-buttons");
        item.appendChild(buttonsDiv);
        const buttonsSpan = document.createElement("span")
        buttonsDiv.appendChild(buttonsSpan);
        const promotionBtn = document.createElement("a");
        promotionBtn.textContent = "Promotion ";
        promotionBtn.onclick = function() { promoteToManager(member.id) };
        buttonsSpan.appendChild(promotionBtn);
        const s = document.createElement("a");
        s.textContent = " / "
        buttonsSpan.appendChild(s);
        const demotionBtn = document.createElement("a");
        demotionBtn.textContent = "Remove";
        demotionBtn.onclick = function() { removeTenantMember(member.id) };
        buttonsSpan.appendChild(demotionBtn);
    })
}

async function showMembersWithNoTenant() {
    const members = await getMembersWithNoTenant();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    const emptyMember = jsonIsEmpty(members);
    if(emptyMember == 1) {
        const emptyText = document.createElement("h2");
        emptyText.textContent = "No member found.";
        mainContainer.appendChild(emptyText);
    }
    members.forEach(member => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2");
        itemTitle.textContent = "(Members)";
        item.appendChild(itemTitle);
        const userNameLabel = document.createElement("label");
        userNameLabel.textContent = "Username: ";
        item.appendChild(userNameLabel);
        const userName = document.createElement("h3");
        userName.textContent = member.userName;
        item.appendChild(userName);
        const emailLabel = document.createElement("label");
        emailLabel.textContent = "Email: ";
        item.appendChild(emailLabel);
        const email = document.createElement("h3");
        email.textContent = member.email;
        item.appendChild(email);
        const joinedAtLabel = document.createElement("label");
        joinedAtLabel.textContent = "Joined at: ";
        item.appendChild(joinedAtLabel);
        const joinedAt = document.createElement("h3");
        joinedAt.textContent = member.joinedAt;
        item.appendChild(joinedAt);
        const promotionBtn = document.createElement("button");
        promotionBtn.textContent = "Add as your member";
        promotionBtn.onclick = function() { addMemberToTenant(member.id) };
        item.appendChild(promotionBtn);
    })
}

async function memberUpdateStyle() {
    const member = await getMember();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item";
    const updateDiv = document.createElement("div");
    updateDiv.classList.add("add-update-container");
    mainContainer.appendChild(updateDiv);
    const userNameLabel = document.createElement("label");
    userNameLabel.textContent = "Username: ";
    updateDiv.appendChild(userNameLabel);
    const userName = document.createElement("input");
    userName.type = "text";
    userName.value = member.userName;
    userName.id = "memberUserName";
    updateDiv.appendChild(userName)
    const emailLabel = document.createElement("label");
    emailLabel.textContent = "Email: ";
    updateDiv.appendChild(emailLabel);
    const adminEmail = document.createElement("input");
    adminEmail.type = "text";
    adminEmail.value = member.email;
    adminEmail.id = "memberEmail";
    updateDiv.appendChild(adminEmail);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Update";
    updateBtn.onclick = function() { updateMember() };
    updateDiv.appendChild(updateBtn);
}

// Project function
async function getProjects() {
    const userInfo = await getUserInfoFromToken();
    try {
        if(userInfo.role === "Tenant") {
            tenantId = userInfo.id;
        } else if(userInfo.tenantId != null) {
            tenantId = userInfo.tenantId;
        }
        const response = await fetch(`${projectUrl}/getprojectswithtenantid`);
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        const projects = await response.json();
        return projects;
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

function addProjectStyle() {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item";
    const addDiv = document.createElement("div");
    addDiv.classList.add("add-update-container");
    mainContainer.appendChild(addDiv);
    const nameLabel = document.createElement("label");
    nameLabel.textContent = "Name: ";
    addDiv.appendChild(nameLabel);
    const nameInput = document.createElement("input");
    nameInput.type = "text";
    nameInput.id = "projectName";
    addDiv.appendChild(nameInput);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    addDiv.appendChild(descriptionLabel);
    const descriptionInput = document.createElement("input");
    descriptionInput.type = "text";
    descriptionInput.id = "projectDescription";
    addDiv.appendChild(descriptionInput);
    const addBtn = document.createElement("button");
    addBtn.textContent = "Add";
    addBtn.onclick = function() { addProject() };
    addDiv.appendChild(addBtn);
}

async function deleteProject(id) {
    const response = await fetch(`${projectUrl}/delete?id=${id}`, {
        method: "DELETE"
    });
    if(response.ok) {
        const projects = await getProjects();
        showProjects(projects);
    }else if(!response.ok) {
        throw new Error(`There was a problem while handling the request: ${response.status}`);
    }
}

async function showProjects() {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    const projects = await getProjects();
    const emptyProjects = jsonIsEmpty(projects);
    if(emptyProjects == 1) {
        const emptyText = document.createElement("h2");
        emptyText.textContent = "No project found.";
        mainContainer.appendChild(emptyText);
    }
    projects.forEach(project => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2");
        itemTitle.textContent = "(Project)";
        item.appendChild(itemTitle);
        const nameLabel = document.createElement("label");
        nameLabel.textContent = "Name: ";
        item.appendChild(nameLabel);
        const name = document.createElement("h3");
        name.textContent = project.name;
        item.appendChild(name);
        const descriptionLabel = document.createElement("label");
        descriptionLabel.textContent = "Description: ";
        item.appendChild(descriptionLabel);
        const description = document.createElement("h3");
        description.textContent = project.description;
        item.appendChild(description);
        const createdAtLabel = document.createElement("label");
        createdAtLabel.textContent = "Created At: ";
        item.appendChild(createdAtLabel);
        const createdAt = document.createElement("h3");
        createdAt.textContent = project.createdAt;
        item.appendChild(createdAt);
        const buttonsDiv = document.createElement("div");
        buttonsDiv.classList.add("item-buttons");
        item.appendChild(buttonsDiv);
        const buttonsSpan = document.createElement("span");
        buttonsDiv.appendChild(buttonsSpan);
        const showBoards = document.createElement("a");
        showBoards.textContent = "Boards";
        showBoards.onclick = function() { getBoardsWithProjectId(project.id)};
        buttonsSpan.appendChild(showBoards);
        const splitterB = document.createElement("a");
        splitterB.classList.add("buttons-splitter");
        splitterB.textContent = " / ";
        buttonsSpan.appendChild(splitterB);
        const addBoardBtn = document.createElement("a");
        addBoardBtn.textContent = "Add board";
        addBoardBtn.onclick = function() { addBoardModal(project.id) };
        buttonsSpan.appendChild(addBoardBtn);
        const splitterC = document.createElement("a");
        splitterC.classList.add("buttons-splitter");
        splitterC.textContent = " / ";
        buttonsSpan.appendChild(splitterC);
        const editProject = document.createElement("a");
        editProject.textContent = "Edit";
        editProject.onclick = function() { openUpdateModal(project.id, "project", project)};
        buttonsSpan.appendChild(editProject);
        const splitterA = document.createElement("a");
        splitterA.classList.add("buttons-splitter");
        splitterA.textContent = "  /  ";
        buttonsSpan.appendChild(splitterA);
        const deleteBtn = document.createElement("a");
        deleteBtn.textContent = "Delete";
        deleteBtn.onclick = function() { deleteProject(project.id) };
        buttonsSpan.appendChild(deleteBtn);
    });
}

// Board functions
async function getBoardsWithProjectId(id) {
    try {
        const response = await fetch(`${boardUrl}/getboardswithprojectid?id=${id}`);
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        const boards = await response.json();
        const boardIsEmpty = jsonIsEmpty(boards);
        if(boardIsEmpty) {
            alert("No board found.");
            return;
        }
        boardStyle(boards, id);
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function deleteBoard(id, projectId) {
    const response = await fetch(`${boardUrl}/deleteboard?id=${id}`, {
        method: "DELETE"
    });
    if(response.ok) {
        getBoardsWithProjectId(projectId);
    } else {
        throw new Error(`There was a problem while handling the request: ${response.status}`);
    }
}

function boardStyle(boards, projectId) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    const backBtn = document.createElement("button");
    backBtn.classList.add("back-button");
    backBtn.textContent = "<-- Back to projects";
    backBtn.onclick = function() { showProjects() };
    mainContainer.appendChild(backBtn);
    boards.forEach(board => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        item.id = board.id;
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2")
        itemTitle.textContent = "(Board)";
        item.appendChild(itemTitle);
        const descriptionLabel = document.createElement("label");
        descriptionLabel.textContent = "Description: ";
        item.appendChild(descriptionLabel);
        const description = document.createElement("h3");
        description.textContent = board.description;
        item.appendChild(description);
        const createdAtLabel = document.createElement("label");
        createdAtLabel.textContent = "Created at: ";
        item.appendChild(createdAtLabel);
        const createdAt = document.createElement("h3");
        createdAt.textContent = board.createdAt;
        item.appendChild(createdAt);
        const buttonsDiv = document.createElement("div");
        buttonsDiv.classList.add("item-buttons");
        item.appendChild(buttonsDiv);
        const buttonsSpan = document.createElement("span");
        buttonsDiv.appendChild(buttonsSpan);
        const showTaskItems = document.createElement("a");
        showTaskItems.textContent = "Related tasks";
        showTaskItems.onclick = function() { getTasksWithBoardId(board.id, board.projectId)};
        buttonsSpan.appendChild(showTaskItems);
        const splitterA = document.createElement("a");
        splitterA.classList.add("buttons-splitter");
        splitterA.textContent = "  /  ";
        buttonsSpan.appendChild(splitterA);
        const addTaskBtn = document.createElement("a");
        addTaskBtn.textContent = "Add task";
        addTaskBtn.onclick = function() { addTaskItemModal(board.id) };
        buttonsSpan.appendChild(addTaskBtn);
        const splitterB = document.createElement("a");
        splitterB.classList.add("buttons-splitter");
        splitterB.textContent = "  /  ";
        buttonsSpan.appendChild(splitterB);
        const editBoard = document.createElement("a");
        editBoard.textContent = "Edit";
        editBoard.onclick = function() { openUpdateModal(board.id, "board", board)};
        buttonsSpan.appendChild(editBoard);
        const splitterC = document.createElement("a");
        splitterC.classList.add("buttons-splitter");
        splitterC.textContent = "  /  ";
        buttonsSpan.appendChild(splitterC);
        const deleteBtn = document.createElement("a");
        deleteBtn.textContent = "Delete";
        deleteBtn.onclick = function() { deleteBoard(board.id, projectId    ) };
        buttonsSpan.appendChild(deleteBtn);
    });
}

// Task functions
async function getTasksWithBoardId(id, projectId) {
    try {
        const response = await fetch(`${taskItemUrl}/gettaskitemwithboardid?id=${id}`);
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        const taskItems = await response.json();
        const taskIsEmpty = jsonIsEmpty(taskItems);
        if(taskIsEmpty) {
            alert("No task found.");
            return;
        }
        taskStyle(taskItems, projectId);
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

function taskStyle(taskItems, projectId) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    const backBtn = document.createElement("button");
    backBtn.classList.add("back-button");
    backBtn.textContent = "<-- Back to boards";
    backBtn.onclick = function() { getBoardsWithProjectId(projectId) };
    mainContainer.appendChild(backBtn);
    taskItems.forEach(taskItem => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        item.id = taskItem.id;
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2");
        itemTitle.textContent = "(Task)";
        item.appendChild(itemTitle);
        const titleLabel = document.createElement("label");
        titleLabel.textContent = "Title: ";
        item.appendChild(titleLabel);
        const title = document.createElement("h3");
        title.textContent = taskItem.title;
        item.appendChild(title);
        const descriptionLabel = document.createElement("label");
        descriptionLabel.textContent = "Description: ";
        item.appendChild(descriptionLabel);
        const description = document.createElement("h3");
        description.textContent = taskItem.description;
        item.appendChild(description);
        const assignedMemberLabel = document.createElement("label");
        assignedMemberLabel.textContent = "Assigned Member: ";
        item.appendChild(assignedMemberLabel);
        const assignedMember = document.createElement("h3");
        assignedMember.textContent = taskItem.assignedMemberName;
        item.appendChild(assignedMember);
        const statusLabel = document.createElement("label");
        statusLabel.textContent = "Status: ";
        item.appendChild(statusLabel);
        const status = document.createElement("h3");
        status.textContent = taskItem.status;
        item.appendChild(status);
        const createdAtLabel = document.createElement("label");
        createdAtLabel.textContent = "Created at: ";
        item.appendChild(createdAtLabel);
        const createdAt = document.createElement("h3");
        createdAt.textContent = taskItem.createdAt;
        item.appendChild(createdAt);
        const buttonsDiv = document.createElement("div");
        buttonsDiv.classList.add("item-buttons");
        item.appendChild(buttonsDiv);
        const buttonsSpan = document.createElement("span");
        buttonsDiv.appendChild(buttonsSpan);
        const editBtn = document.createElement("a");
        editBtn.textContent = "Edit board";
        editBtn.onclick = function() { openUpdateModal(taskItem.id, "taskItem", taskItem) };
        buttonsSpan.appendChild(editBtn);
        const splitter = document.createElement("a");
        splitter.classList.add("buttons-splitter");
        splitter.textContent = "  /  ";
        buttonsSpan.appendChild(splitter)
        const deleteBtn = document.createElement("a");
        deleteBtn.textContent = "Delete";
        deleteBtn.onclick = function() { deleteTaskItem(taskItem.id, taskItem.boardId) };
        buttonsSpan.appendChild(deleteBtn);
    });
}

async function getMemberTasks() {
    const taskItems = await getTaskItemsWithMemberId();
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-box";
    const countTaskItems = jsonIsEmpty(taskItems);
    if(countTaskItems == true) {
        const noTaskText = document.createElement("h2");
        noTaskText.textContent = "You don't have any task at the moment.";
        mainContainer.appendChild(noTaskText);
    }
    taskItems.forEach(taskItem => {
        const item = document.createElement("div");
        item.classList.add("container-item");
        item.id = taskItem.id;
        mainContainer.appendChild(item);
        const itemTitle = document.createElement("h2");
        itemTitle.textContent = "(Task)";
        item.appendChild(itemTitle);
        const titleLabel = document.createElement("label");
        titleLabel.textContent = "Title: ";
        item.appendChild(titleLabel);
        const title = document.createElement("h3");
        title.textContent = taskItem.title;
        item.appendChild(title);
        const descriptionLabel = document.createElement("label");
        descriptionLabel.textContent = "Description: ";
        item.appendChild(descriptionLabel);
        const description = document.createElement("h3");
        description.textContent = taskItem.description;
        item.appendChild(description);
        const assignedMemberLabel = document.createElement("label");
        assignedMemberLabel.textContent = "Assigned Member: ";
        item.appendChild(assignedMemberLabel);
        const assignedMember = document.createElement("h3");
        assignedMember.textContent = taskItem.assignedMemberName;
        item.appendChild(assignedMember);
        const statusLabel = document.createElement("label");
        statusLabel.textContent = "Status: ";
        item.appendChild(statusLabel);
        const status = document.createElement("h3");
        status.textContent = taskItem.status;
        item.appendChild(status);
        const createdAtLabel = document.createElement("label");
        createdAtLabel.textContent = "Created at: ";
        item.appendChild(createdAtLabel);
        const createdAt = document.createElement("h3");
        createdAt.textContent = taskItem.createdAt;
        item.appendChild(createdAt);
        const buttonsDiv = document.createElement("div");
        buttonsDiv.classList.add("item-buttons");
        item.appendChild(buttonsDiv);
        const buttonsSpan = document.createElement("span");
        buttonsDiv.appendChild(buttonsSpan);
        const editBtn = document.createElement("a");
        editBtn.textContent = "Edit board";
        editBtn.onclick = function() { openUpdateModal(taskItem.id, "taskItem", taskItem) };
        buttonsSpan.appendChild(editBtn);
        const splitter = document.createElement("a");
        splitter.textContent = "  /  ";
        buttonsSpan.appendChild(splitter)
        const deleteBtn = document.createElement("a");
        deleteBtn.textContent = "Delete";
        deleteBtn.onclick = function() { deleteTaskItem(taskItem.id, taskItem.boardId) };
        buttonsSpan.appendChild(deleteBtn);
    });
}

function jsonIsEmpty(jsn) {
    return Object.keys(jsn).length === 0;
}