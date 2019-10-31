/*
Navicat MySQL Data Transfer

Source Server         : 121.43.107.164
Source Server Version : 50173
Source Host           : 121.43.107.164:3306
Source Database       : tslog

Target Server Type    : MYSQL
Target Server Version : 50173
File Encoding         : 65001

Date: 2018-11-19 11:37:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `backup_t`
-- ----------------------------
DROP TABLE IF EXISTS `backup_t`;
CREATE TABLE `backup_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`)
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=167

;

-- ----------------------------
-- Table structure for `equip_t`
-- ----------------------------
DROP TABLE IF EXISTS `equip_t`;
CREATE TABLE `equip_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=1

;

-- ----------------------------
-- Table structure for `huodong_t`
-- ----------------------------
DROP TABLE IF EXISTS `huodong_t`;
CREATE TABLE `huodong_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=1

;

-- ----------------------------
-- Table structure for `item_t`
-- ----------------------------
DROP TABLE IF EXISTS `item_t`;
CREATE TABLE `item_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=1

;

-- ----------------------------
-- Table structure for `login_t`
-- ----------------------------
DROP TABLE IF EXISTS `login_t`;
CREATE TABLE `login_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) ,
INDEX `player_guid_index` USING BTREE (`player_guid`) ,
INDEX `dt_index` USING BTREE (`dt`) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=4278077

;

-- ----------------------------
-- Table structure for `resource_t`
-- ----------------------------
DROP TABLE IF EXISTS `resource_t`;
CREATE TABLE `resource_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=1

;

-- ----------------------------
-- Table structure for `role_t`
-- ----------------------------
DROP TABLE IF EXISTS `role_t`;
CREATE TABLE `role_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=1

;

-- ----------------------------
-- Table structure for `statistics_t`
-- ----------------------------
DROP TABLE IF EXISTS `statistics_t`;
CREATE TABLE `statistics_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=1

;

-- ----------------------------
-- Table structure for `treasure_t`
-- ----------------------------
DROP TABLE IF EXISTS `treasure_t`;
CREATE TABLE `treasure_t` (
`id`  bigint(20) NOT NULL AUTO_INCREMENT ,
`username`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`serverid`  text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,
`player_guid`  bigint(20) NOT NULL DEFAULT 0 ,
`value1`  int(11) NOT NULL DEFAULT 0 ,
`value2`  int(11) NOT NULL DEFAULT 0 ,
`value3`  int(11) NOT NULL DEFAULT 0 ,
`value4`  int(11) NOT NULL DEFAULT 0 ,
`value5`  int(11) NOT NULL DEFAULT 0 ,
`pt`  text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`dt`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `username_index` USING BTREE (`username`(32)) ,
INDEX `serverid_index` USING BTREE (`serverid`(16)) 
)
ENGINE=MyISAM
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=1

;

-- ----------------------------
-- Auto increment value for `backup_t`
-- ----------------------------
ALTER TABLE `backup_t` AUTO_INCREMENT=167;

-- ----------------------------
-- Auto increment value for `equip_t`
-- ----------------------------
ALTER TABLE `equip_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for `huodong_t`
-- ----------------------------
ALTER TABLE `huodong_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for `item_t`
-- ----------------------------
ALTER TABLE `item_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for `login_t`
-- ----------------------------
ALTER TABLE `login_t` AUTO_INCREMENT=4278077;

-- ----------------------------
-- Auto increment value for `resource_t`
-- ----------------------------
ALTER TABLE `resource_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for `role_t`
-- ----------------------------
ALTER TABLE `role_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for `statistics_t`
-- ----------------------------
ALTER TABLE `statistics_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for `treasure_t`
-- ----------------------------
ALTER TABLE `treasure_t` AUTO_INCREMENT=1;
