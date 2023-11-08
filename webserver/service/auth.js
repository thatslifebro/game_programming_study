import pool from '../db/mysql.js';
import bcrypt from 'bcrypt';
const saltRounds = 10;



import signToken from '../util/sign-token.js';

export default class authService {
  static async test() {
    const conn = await pool.promise().getConnection();
    const [rows, fields] = await conn.query("SELECT * FROM user");
    console.log(rows);
    return rows;
  }

  static async login(email, password) {
    const user = await User.findOne({ email });
    if (!user) {
      throw new Error('이메일이나 비번이 틀림');
    }
    const match = await bcrypt.compare(password, user.password);
    if (!match) {
      throw new Error('이메일이나 비번이 틀림');
    }
    const token = await signToken(user.id, user.role);
    return token;
  }

  static async register({ nickname, email,  password, etc }) {
    const conn = await pool.promise().getConnection();
    let rows, fields;
    [rows, fields] = await conn.query(`SELECT * FROM user WHERE nickname="${nickname}"`);
    if(rows[0]){
      throw new Error("사용 중인 닉네임 입니다.");
    }
    [rows, fields] = await conn.query(`SELECT * FROM user WHERE email="${email}"`);
    if(rows[0]){
      throw new Error("사용 중인 이메일 입니다.");
    }
    const now = Date();
    console.log(now);
    // const today = now.
    // [rows, fields] = await conn.query(`INSERT INTO user (nickname, email, register_date, recent_login_date, etc, enc_pw, rating) VALUES( "${nickname}", "${email}", "${Date.now()}","${}","${}", "${}","${}")`);

    // console.log(rows);
    return rows;
    

    const user = await User.findOne({ email });
    if (user) {
      throw new Error('이미 있는 이메일');
    }
    const newUser = await User.create({
      email,
      address,
      fullName,
      password: await bcrypt.hash(password, saltRounds),
    });
    const filtered = {
      email: newUser.email,
      address: newUser.address,
      fullName: newUser.fullName,
      role: newUser.role,
    };
    return filtered;
  }

  static async withdrawal({ userId, role, password }) {
    if (role !== 'USER') {
      throw new Error('권한이 없다');
    }
    const user = await User.findOne({ _id: userId });
    const match = await bcrypt.compare(password, user.password);
    if (!match) {
      throw new Error('비번이 틀림');
    }
    const deletedUser = await User.findByIdAndDelete(userId);
    const filtered = {
      email: deletedUser.email,
      address: deletedUser.address,
      fullName: deletedUser.fullName,
      role: deletedUser.role,
    };
    return filtered;
  }
}