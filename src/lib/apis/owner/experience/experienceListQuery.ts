import { createAsyncThunk } from '@reduxjs/toolkit';
import { dynamicApi } from '../../apiClient';

export const experienceListQuery = createAsyncThunk(
    'experience/experienceListQuery',
    async (_, thunkAPI)  => {
        try {

            const response = await dynamicApi({
                method: 'GET',
                url: '/Owner/ExperienceList',
                withCredentials: true
            });

            if (response.status === 204) return [];

            return [...response.data.items];

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);
