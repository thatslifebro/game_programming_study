import mysql from "mysql2";
import { DB_INFO } from "../constants/env.js";

const pool = mysql.createPool(DB_INFO);




export default pool;