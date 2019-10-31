/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50724
Source Host           : localhost:3306
Source Database       : tsuser

Target Server Type    : MYSQL
Target Server Version : 50724
File Encoding         : 65001

Date: 2018-11-23 12:32:33
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `user`
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `pid` int(11) NOT NULL AUTO_INCREMENT,
  `username` text NOT NULL,
  `password` text NOT NULL,
  `mail` text,
  `is_reg` int(11) DEFAULT NULL,
  PRIMARY KEY (`pid`),
  KEY `username_index` (`username`(8)) USING BTREE,
  KEY `mail_index` (`mail`(8)) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

