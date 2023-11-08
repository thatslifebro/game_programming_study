import mysql from "mysql2";
import { DB_URL } from "../constants/env.js";

const pool = mysql.createPool(DB_URL);

export default pool;