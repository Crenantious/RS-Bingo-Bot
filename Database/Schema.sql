 CREATE TABLE Team (
    RowId int AUTO_INCREMENT,
    Name varchar(50) NOT NULL,
    BoardChannelId bigint NOT NULL,
    PRIMARY KEY (RowId)
);

 CREATE TABLE User (
    DiscordUserId bigint PRIMARY KEY,
    TeamID int NOT NULL,
    BoardChannelId bigint NOT NULL,
	FOREIGN KEY (TeamID) REFERENCES Team(RowId)
);

 CREATE TABLE Task (
    RowID int PRIMARY KEY AUTO_INCREMENT,
    Name varchar(50) NOT NULL
);

 CREATE TABLE Restriction (
    RowID int PRIMARY KEY AUTO_INCREMENT,
    Name varchar(50) NOT NULL UNIQUE
);

CREATE TABLE TaskRestriction (
    TaskID int NOT NULL,
    RestrictionID int NOT NULL,
    PRIMARY KEY (TaskID, RestrictionID),
    CONSTRAINT `Constr_TaskRestriction_Task_fk`
        FOREIGN KEY Task_fk (TaskID) REFERENCES Task(RowID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `Constr_TaskRestriction_Restriction_fk`
        FOREIGN KEY Restriction_fk (RestrictionID) REFERENCES Restriction(RowID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

 CREATE TABLE Tile (
    RowID int PRIMARY KEY AUTO_INCREMENT,
	TeamID int NOT NULL,
	TaskID int NOT NULL,
    FOREIGN KEY (TeamID) REFERENCES Team(RowID),
    FOREIGN KEY (TaskID) REFERENCES Task(RowID),
	CONSTRAINT team_task_relationship UNIQUE KEY (TeamID, TaskID),
	CONSTRAINT task_team_relationship UNIQUE KEY (TaskID, TeamID)
);
  
 CREATE TABLE Evidence (
    RowID int PRIMARY KEY AUTO_INCREMENT,
	TeamID int NOT NULL,
	DiscordUserId bigint NOT NULL,
    URL varchar(255) NOT NULL,
	`Status` tinyint NOT NULL,
    `Type` tinyint NOT NULL,
    FOREIGN KEY (TeamID) REFERENCES Team(RowID),
    FOREIGN KEY (DiscordUserId) REFERENCES `User`(DiscordUserId)
);