import express from 'express';
import cors from 'cors';
import authRouter from './router/auth.js';
import usersRouter from './router/users.js';
import { SERVER_PORT } from './constants/env.js';

const app = express();

app.use(cors());

//req.body로 데이터 받아오려면 써야하는 것
app.use(express.json());
app.use(express.urlencoded({ extended: false }));

//라우터
app.use('/api/auth', authRouter);
app.use('/api/users', usersRouter);

app.use((err, req, res, next) => {
  console.log(err);
  res.send(err.message);
});

//서버열기 'localhost:3001'
app.listen(3001, (req, res) => {
  console.log('시작');
});