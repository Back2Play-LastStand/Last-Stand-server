CREATE DATABASE IF NOT EXISTS last_stand_account_data;
USE last_stand_account_data;

DROP TABLE IF EXISTS `player_account_data`;
CREATE TABLE `player_account_data` (
  `id` int NOT NULL AUTO_INCREMENT,
  `player_id` varchar(50) NOT NULL,
  `password` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `is_new_account` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `player_id` (`player_id`),
  UNIQUE KEY `UQ_Email` (`email`)
);

DROP TABLE IF EXISTS `account_session`;
CREATE TABLE `account_session` (
  `session_id` varchar(64) NOT NULL,
  `account_id` int NOT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `expires_at` datetime NOT NULL,
  PRIMARY KEY (`session_id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_session_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `player_account_data` (`id`)
);

CREATE DATABASE IF NOT EXISTS last_stand_game_data;
USE last_stand_game_data;

DROP TABLE IF EXISTS `player_data`;
CREATE TABLE `player_data` (
  `account_id` int NOT NULL,
  `player_id` varchar(50) NOT NULL,
  `player_name` varchar(64) NOT NULL,
  UNIQUE KEY `player_id` (`player_id`),
  UNIQUE KEY `player_name` (`player_name`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `player_data_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `last_stand_account_data`.`player_account_data` (`id`),
  CONSTRAINT `player_data_ibfk_2` FOREIGN KEY (`player_id`) REFERENCES `last_stand_account_data`.`player_account_data` (`player_id`)
);