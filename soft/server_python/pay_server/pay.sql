DROP TABLE IF EXISTS `pay_t`;
CREATE TABLE  `pay_t` (
  `id` int(11) auto_increment NOT NULL,
  `username` varchar(128) DEFAULT NULL,
  `serverid` varchar(128) DEFAULT NULL,
  `rid` int(11) NOT NULL,
  `orderid` varchar(128) DEFAULT NULL,
  `res` int(11) NOT NULL,
  `dt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `username` (`username`),
  KEY `orderid` (`orderid`(32))
) ENGINE=InnoDB DEFAULT CHARSET=utf8; 



