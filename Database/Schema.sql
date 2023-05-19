CREATE TABLE Team (
    RowID int PRIMARY KEY AUTO_INCREMENT,
    Name varchar(50) NOT NULL UNIQUE,
    CategoryChannelID bigint UNSIGNED NOT NULL,
    BoardChannelID bigint UNSIGNED NOT NULL,
    GeneralChannelID bigint UNSIGNED NOT NULL,
    EvidenceChannelID bigint UNSIGNED NOT NULL,
    VoiceChannelID bigint UNSIGNED NOT NULL,
    BoardMessageID bigint UNSIGNED NOT NULL,
    RoleID bigint UNSIGNED NOT NULL,
    Score int NOT NULL
);

CREATE TABLE `User` (
    DiscordUserID bigint UNSIGNED PRIMARY KEY,
    TeamID int NOT NULL,
	FOREIGN KEY (TeamID) REFERENCES Team(RowID)
		ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Task (
    RowID int PRIMARY KEY AUTO_INCREMENT,
    Name varchar(50) NOT NULL,
    Difficulty tinyint NOT NULL
);

CREATE TABLE Restriction (
    RowID int PRIMARY KEY AUTO_INCREMENT,
    Name varchar(50) NOT NULL,
    Description varchar(50) NOT NULL UNIQUE
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
    IsVerified tinyint NOT NULL,
    BoardIndex int NOT NULL,
    IsComplete tinyint NOT NULL,
    FOREIGN KEY (TeamID) REFERENCES Team(RowID)
		ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (TaskID) REFERENCES Task(RowID)
		ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT team_task_relationship UNIQUE KEY (TeamID, TaskID),
	CONSTRAINT task_team_relationship UNIQUE KEY (TaskID, TeamID),
	CONSTRAINT team_boardIndex_relationship UNIQUE KEY (TeamID, BoardIndex),
	CONSTRAINT boardIndex_team_relationship UNIQUE KEY (BoardIndex, TeamID)
);

CREATE TABLE Evidence (
    RowID int PRIMARY KEY AUTO_INCREMENT,
	TileID int NOT NULL,
	DiscordUserID bigint UNSIGNED NOT NULL,
    DiscordMessageID bigint UNSIGNED NOT NULL,
    URL varchar(255) NOT NULL,
	`Status` tinyint NOT NULL,
    `Type` tinyint NOT NULL,
    FOREIGN KEY (TileID) REFERENCES Tile(RowID)
		ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (DiscordUserID) REFERENCES `User`(DiscordUserID)
		ON DELETE CASCADE ON UPDATE CASCADE
);