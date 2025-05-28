import { ContactMessageFormData } from "@/lib/schemas";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const sendEmail = createAsyncThunk(
    'client/sendEmail',
    async (payload: ContactMessageFormData, thunkAPI) => {
        try {
            const res = await fetch('https://localhost:7206/api/Client/sendEmail', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });

            if (!res.ok){
                var error = await res.json();
                return thunkAPI.rejectWithValue(error);
            }
            
            return;

        } catch (error) {
            return thunkAPI.rejectWithValue(['Unexpected error occurred.']);
        }
    }
);