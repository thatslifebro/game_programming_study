import { Router } from 'express';
import asyncHandler from '../util/async-handler.js';
import authService from '../service/auth.js';

import verifyToken from '../middleware/verify-token.js';
import jwt from 'jsonwebtoken';
const { verify } = jwt;

const router = Router();

router.get(
    '/test',
    asyncHandler(async (req,res)=>{
        // const { nickname } = req.body;
        const a = await authService.test(); 
        return res.status(200).json(a);
    }),
);

//로그인
router.post(
  '/login',
  asyncHandler(async (req, res) => {
    const { email, password } = req.body;
    const token = await authService.login(email, password);
    return res.status(200).json(token);
  }),
);

//회원가입
router.post(
  '/register',
  asyncHandler(async (req, res) => {
    const { nickname, email, password, etc } = req.body;
    const createdUser = await authService.register({
      nickname,
      email,
      password,
      etc,
    });
    return res.status(201).json(createdUser);
  }),
);

//회원탈퇴
router.delete(
  '/withdrawal',
  verifyToken,
  asyncHandler(async (req, res) => {
    const { userId, role } = req.decoded;
    const { password } = req.body;
    const deletedUser = await authService.withdrawal({
      userId,
      role,
      password,
    });
    return res.status(200).json(deletedUser);
  }),
);

router.get(
  '/verify',
  verifyToken,
  asyncHandler(async (req, res) => {
    const { role } = req.decoded;
    return res.status(200).json({ role });
  }),
);

export default router;