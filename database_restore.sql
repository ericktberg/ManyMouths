-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: localhost    Database: many_mouths
-- ------------------------------------------------------
-- Server version	8.0.34

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `good`
--

DROP TABLE IF EXISTS `store_location`;
DROP TABLE IF EXISTS `store_chain`;
DROP TABLE IF EXISTS `recipe_owner`;
DROP TABLE IF EXISTS `recipe`;
DROP TABLE IF EXISTS `recipe_quant`;
DROP TABLE IF EXISTS `good_transaction`;
DROP TABLE IF EXISTS `ingredient_mapping`;
DROP TABLE IF EXISTS `good`;
DROP TABLE IF EXISTS `ingredient`;
DROP TABLE IF EXISTS `mapping_owner`;
DROP TABLE IF EXISTS `user`;

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `good` (
  `good_id` BINARY(16) NOT NULL,
  `friendly_name` varchar(45) DEFAULT NULL COMMENT 'A friendly name to assign the good. An optional field, that can be updated later.',
  `store_code` int unsigned NOT NULL COMMENT 'The PLU or UPC code associated with the good',
  `code_type` int unsigned NOT NULL COMMENT '1 = PLU\n2 = UPC\n\nDeclares how to interpret store_code',
  PRIMARY KEY (`good_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `good_transaction`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `good_transaction` (
  `good_transaction_id` int unsigned NOT NULL AUTO_INCREMENT,
  `good_id` BINARY(16) NOT NULL,
  `store_location_id` int unsigned NOT NULL,
  `price` int unsigned NOT NULL COMMENT 'Price in cents. Divide by 100 to get dollar cost.',
  `unit` int DEFAULT NULL COMMENT 'The unit to which the price applies\n0 = Each\n1 = Pound\n2 = Oz (weight)',
  PRIMARY KEY (`good_transaction_id`),
  KEY `good_id_idx` (`good_id`),
  KEY `store_location_id` (`store_location_id`),
  CONSTRAINT `good_id` FOREIGN KEY (`good_id`) REFERENCES `good` (`good_id`) ON DELETE RESTRICT,
  CONSTRAINT `store_location_id` FOREIGN KEY (`store_location_id`) REFERENCES `store_location` (`store_location_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ingredient`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingredient` (
  `ingredient_id` BINARY(16) NOT NULL,
  `ingredient_name` varchar(45) NOT NULL,
  PRIMARY KEY (`ingredient_id`),
  UNIQUE KEY `ingredient_name_UNIQUE` (`ingredient_name`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ingredient_mapping`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingredient_mapping` (
  `mapping_id` int unsigned NOT NULL AUTO_INCREMENT,
  `good_id` BINARY(16) NOT NULL,
  `ingredient_id` BINARY(16) NOT NULL,
  PRIMARY KEY (`mapping_id`),
  UNIQUE KEY `mapping` (`good_id`,`ingredient_id`) /*!80000 INVISIBLE */,
  KEY `ingredient_mapping_ingredient_id_idx` (`ingredient_id`),
  CONSTRAINT `ingredient_mapping_good_id` FOREIGN KEY (`good_id`) REFERENCES `good` (`good_id`),
  CONSTRAINT `ingredient_mapping_ingredient_id` FOREIGN KEY (`ingredient_id`) REFERENCES `ingredient` (`ingredient_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mapping_owner`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mapping_owner` (
  `mapping_id` int unsigned NOT NULL,
  `user_id` BINARY(16) NOT NULL,
  PRIMARY KEY (`user_id`,`mapping_id`),
  KEY `mapping_owner_mapping_id` (`mapping_id`),
  CONSTRAINT `mapping_owner_mapping_id` FOREIGN KEY (`mapping_id`) REFERENCES `ingredient_mapping` (`mapping_id`),
  CONSTRAINT `mapping_owner_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipe` (
  `recipe_id` BINARY(16) NOT NULL,
  `recipe_name` varchar(45) NOT NULL,
  PRIMARY KEY (`recipe_id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_owner`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipe_owner` (
  `recipe_id` BINARY(16) NOT NULL,
  `user_id` BINARY(16) NOT NULL,
  PRIMARY KEY (`recipe_id`,`user_id`),
  KEY `user_id_idx` (`user_id`),
  CONSTRAINT `recipe_owner_recipe_id` FOREIGN KEY (`recipe_id`) REFERENCES `recipe` (`recipe_id`),
  CONSTRAINT `recipe_owner_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_quant`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipe_quant` (
  `recipe_id` BINARY(16) NOT NULL,
  `ingredient_id` BINARY(16) NOT NULL,
  `quantity` int unsigned NOT NULL COMMENT 'Divide by 100 for decimal quantity.',
  `unit` int unsigned NOT NULL COMMENT '0 = Each\n1 = Pound\n2 = Ounce (weight)',
  PRIMARY KEY (`recipe_id`,`ingredient_id`),
  KEY `ingredient_id_idx` (`ingredient_id`),
  CONSTRAINT `ingredient_id` FOREIGN KEY (`ingredient_id`) REFERENCES `ingredient` (`ingredient_id`),
  CONSTRAINT `recipe_id` FOREIGN KEY (`recipe_id`) REFERENCES `recipe` (`recipe_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='What ingredients and how much of them are used in a recipe';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `store_chain`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `store_chain` (
  `store_chain_id` BINARY(16) NOT NULL,
  `chain_name` varchar(45) NOT NULL COMMENT 'The name of the chain, e.g. Whole Foods, Cub Foods',
  PRIMARY KEY (`store_chain_id`),
  UNIQUE KEY `chain_name_UNIQUE` (`chain_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `store_location`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `store_location` (
  `store_location_id` BINARY(16) NOT NULL AUTO_INCREMENT,
  `store_chain_id` BINARY(16) NOT NULL COMMENT 'A reference to a certain chain that this location is franchising',
  `location_address` varchar(45) DEFAULT NULL COMMENT 'The address of this franchise address',
  `location_number` int NOT NULL COMMENT 'A unique store number will be given to chain franchises.',
  PRIMARY KEY (`store_location_id`),
  UNIQUE KEY `franchise` (`store_chain_id`,`location_number`),
  KEY `store_chain_id_idx` (`store_chain_id`) /*!80000 INVISIBLE */,
  CONSTRAINT `store_chain_id` FOREIGN KEY (`store_chain_id`) REFERENCES `store_chain` (`store_chain_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user`
--

/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_id` BINARY(16) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-10-03 19:59:25
