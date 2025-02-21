import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import agent from '../data/agent';
import { User } from '../models/User';
import { FieldValues } from 'react-hook-form';

export interface UserState {
  user: User | null;
}

const savedUser = localStorage.getItem('user');
const initialState: UserState = {
  user: savedUser ? JSON.parse(savedUser) : null,
};

export const logIn = createAsyncThunk<User, FieldValues>(
  'auth/logIn',
  async (arg, thunkAPI) => {
    try {
      const user = await agent.Auth.login(arg);
      localStorage.setItem('user', JSON.stringify(user));
      localStorage.setItem('userId', JSON.stringify(user.userId));

      return user;
    } catch (error) {
      return thunkAPI.rejectWithValue('Došlo je do greške');
    }
  },
);
export const currentUser = createAsyncThunk<User>(
  'account/currentUser',
  async (_, thunkAPI) => {
    thunkAPI.dispatch(setUser(JSON.parse(localStorage.getItem('user')!)));
    try {
      const user = await agent.Auth.currentUser();
      localStorage.setItem('user', JSON.stringify(user));
      return user;
    } catch (error) {
      return thunkAPI.rejectWithValue('Došlo je do greške');
    }
  },
  {
    condition: () => {
      if (!localStorage.getItem('user')) return false;
    },
  },
);

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setUser(state, action) {
      state.user = action.payload;
    },
    clearUser(state) {
      state.user = null;
      localStorage.removeItem('user');
    },
  },
  extraReducers: (builder) => {
    builder.addCase(logIn.fulfilled, (state, action) => {
      state.user = action.payload;
    });
    builder.addCase(currentUser.fulfilled, (state, action) => {
      state.user = action.payload;
    });
    builder.addCase(logIn.rejected, (state) => {
      state.user = null;
    });
    builder.addCase(currentUser.rejected, (state) => {
      state.user = null;
    });
  },
});

const { actions, reducer } = authSlice;

export const { setUser, clearUser } = actions;
export default reducer;
