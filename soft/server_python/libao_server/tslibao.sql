/*
Navicat MySQL Data Transfer

Source Server         : 121.43.107.164
Source Server Version : 50173
Source Host           : 121.43.107.164:3306
Source Database       : tslibao

Target Server Type    : MYSQL
Target Server Version : 50173
File Encoding         : 65001

Date: 2018-06-20 10:50:49
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `keyword_log`
-- ----------------------------
DROP TABLE IF EXISTS `keyword_log`;
CREATE TABLE `keyword_log` (
`id`  int(11) NOT NULL AUTO_INCREMENT ,
`username`  varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL ,
`code`  varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL ,
`code_type`  varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL ,
`date`  timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP ,
PRIMARY KEY (`id`),
INDEX `ip` USING BTREE (`username`) 
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for `libao`
-- ----------------------------
DROP TABLE IF EXISTS `libao`;
CREATE TABLE `libao` (
`code`  varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`type`  varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`used`  int(11) NOT NULL DEFAULT 0 ,
`wexin`  int(11) NOT NULL ,
PRIMARY KEY (`code`),
INDEX `code_index` USING BTREE (`code`(4)) ,
INDEX `type_index` USING BTREE (`type`) 
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for `libao_type`
-- ----------------------------
DROP TABLE IF EXISTS `libao_type`;
CREATE TABLE `libao_type` (
`code`  varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`name`  varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL ,
`pc`  int(11) NULL DEFAULT NULL ,
`num`  int(11) NULL DEFAULT NULL ,
`type`  blob NULL ,
`value1`  blob NULL ,
`value2`  blob NULL ,
`value3`  blob NULL ,
`dt`  timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP ,
`chongzhi`  int(11) NULL DEFAULT NULL ,
`gongxiang`  int(11) NOT NULL ,
PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
