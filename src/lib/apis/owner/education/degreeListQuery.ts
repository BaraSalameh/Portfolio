import { createAsyncThunk } from '@reduxjs/toolkit';
import { dynamicApi } from '../../apiClient';

export const degreeListQuery = createAsyncThunk(
    'education/degreeListQuery',
    async (_, thunkAPI)  => {
        try {

            const response = await dynamicApi({
                method: 'GET',
                url: '/Owner/LKP_DegreeList',
                withCredentials: true
            });

            if (response.status === 204) return [];

            return [...response.data.items];

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);
