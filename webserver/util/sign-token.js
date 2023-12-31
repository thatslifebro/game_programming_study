import jwt from 'jsonwebtoken';
import { JWT_SECRET } from '../constants/env.js';

const signToken = async (id, role) => {
  const token = await jwt.sign(
    {
      userId: id,
      role: role,
    },
    JWT_SECRET,
    {
      expiresIn: '100h', //1분
      issuer: 'thatslifebro',
    },
  );
  return token;
};

export default signToken;