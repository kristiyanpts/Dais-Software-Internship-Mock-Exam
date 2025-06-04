CREATE DATABASE WorkspaceReservations;
GO

USE WorkspaceReservations;

CREATE TABLE users(
    id INT NOT NULL PRIMARY KEY IDENTITY,
	email NVARCHAR(255) NOT NULL UNIQUE,
    username NVARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(64) NOT NULL, -- SHA-256 hash
	full_name NVARCHAR(255) NOT NULL,
);

CREATE TABLE workspaces(
    id INT NOT NULL PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    floor INT NOT NULL,
    zone NVARCHAR(255) NOT NULL,
    has_monitor BIT NOT NULL,
    has_docking_station BIT NOT NULL,
    is_near_window BIT NOT NULL,
    is_near_printer BIT NOT NULL,
    is_taken BIT NOT NULL DEFAULT 0
);

CREATE TABLE reservations(
    id INT NOT NULL PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    workspace_id INT NOT NULL,
    reservation_date DATE NOT NULL,
    is_quick_reservation BIT NOT NULL DEFAULT 0,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (workspace_id) REFERENCES workspaces(id)
);

CREATE TABLE favorite_workspaces(
    id INT NOT NULL PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    workspace_id INT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (workspace_id) REFERENCES workspaces(id),
    UNIQUE (user_id, workspace_id)
);

CREATE INDEX IX_Users_Email ON users(email);
CREATE INDEX IX_Users_Username ON users(username);
CREATE INDEX IX_Workspaces_Floor ON workspaces(floor);
CREATE INDEX IX_Workspaces_Zone ON workspaces(zone);
CREATE INDEX IX_Reservations_ReservationDate ON reservations(reservation_date);
CREATE INDEX IX_FavoriteWorkspaces_UserId ON favorite_workspaces(user_id);