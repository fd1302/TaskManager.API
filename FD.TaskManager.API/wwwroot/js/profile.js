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
    const editProfile = document.getElementById("editProfile");
    if(info.role === "Tenant") {
        editProfile.onclick = function() { tenantUpdateStyle() };
    }
    if(info.role != "Tenant") {
        const projects = document.getElementById("projects");
        const boards = document.getElementById("boards");
        projects.style.display = "none";
        boards.style.display = "none";
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
        const response = await fetch(`${tenantUrl}/updatetenant`, {
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
    tenantUpdate.classList.add("tenant-update");
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

function managerInfoStyle(info) {
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

// Project function
async function getProjects() {
    const userInfo = await getUserInfoFromToken();
    let tenantId;
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
        projectsStyle(projects);
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

function projectsStyle(projects) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-pbt";
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
        const editProject = document.createElement("a");
        editProject.textContent = "Edit project  ";
        editProject.onclick = function() { openUpdateModal(project.id, "project", project)};
        buttonsSpan.appendChild(editProject);
        const p = document.createElement("a");
        p.textContent = "  /  ";
        buttonsSpan.appendChild(p);
        const showBoards = document.createElement("a");
        showBoards.textContent = "  Related boards";
        showBoards.onclick = function() { getBoardsWithProjectId(project.id)};
        buttonsSpan.appendChild(showBoards);
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
        boardStyle(boards);
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

function boardStyle(boards) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-pbt";
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
        const editBoard = document.createElement("a");
        editBoard.textContent = "Edit board  ";
        editBoard.onclick = function() { openUpdateModal(board.id, "board", board)};
        buttonsSpan.appendChild(editBoard);
        const p = document.createElement("a");
        p.textContent = "  /  ";
        buttonsSpan.appendChild(p);
        const showTaskItems = document.createElement("a");
        showTaskItems.textContent = "  Related tasks";
        showTaskItems.onclick = function() { getTasksWithBoardId(board.id)};
        buttonsSpan.appendChild(showTaskItems);
    })
}

// Task functions
async function getTasksWithBoardId(id) {
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
        taskStyle(taskItems);
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

function taskStyle(taskItems) {
    const mainContainer = document.getElementById("mainContainerItem");
    mainContainer.innerHTML = "";
    mainContainer.className = "main-container-item-pbt";
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
        const editBtn = document.createElement("button");
        editBtn.textContent = "Edit board";
        editBtn.onclick = function() { openUpdateModal(taskItem.id, "taskItem", taskItem) };
        item.appendChild(editBtn);
    })
}

function jsonIsEmpty(jsn) {
    return Object.keys(jsn).length === 0;
}