import { combineReducers } from "redux";
import authSlice from "./slices/authSlice";
import searchSlice from "./slices/searchSlice";
import userSlice from "./slices/userSlice";


const rootReducer = combineReducers({
    auth: authSlice,
    search: searchSlice,
    user: userSlice
});

export default rootReducer;