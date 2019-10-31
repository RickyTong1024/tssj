/*
Navicat MySQL Data Transfer

Source Server         : 192.168.2.66
Source Server Version : 50641
Source Host           : 192.168.2.66:3306
Source Database       : ts_storage

Target Server Type    : MYSQL
Target Server Version : 50641
File Encoding         : 65001

Date: 2018-11-19 20:59:13
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `storage`
-- ----------------------------
DROP TABLE IF EXISTS `storage`;
CREATE TABLE `storage` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `token` text NOT NULL,
  `serverid` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `token_index` (`token`(8))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of storage
-- ----------------------------
