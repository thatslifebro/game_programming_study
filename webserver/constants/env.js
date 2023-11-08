import dotenv from 'dotenv';
dotenv.config();

export const SERVER_PORT = process.env.SERVER_PORT;
// export const DB_ADDRESS = process.env.DB_ADDRESS;
export const JWT_SECRET = process.env.JWT_SECRET;
// export const CLIENT_ADDRESS = process.env.CLIENT_ADDRESS;
export const DB_INFO = {
    host: process.env.DB_HOST,
    port: process.env.DB_PORT,
    user: process.env.DB_USER,
    password: process.env.DB_PW,
    database: process.env.DB_DB,
    ssl:{"rejectUnauthorized":true},
    connectionLimit: 10,
}
