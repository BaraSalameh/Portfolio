import { validateToken } from '../apis/account/validateToken';
import { useAppDispatch, useAppSelector } from '../store/hooks';

export const useCheckAndGetUsername = () => {
    const dispatch = useAppDispatch();
    const username = useAppSelector(state => state.auth.username);

    const getUsername = async (): Promise<string | null> => {
        if (username) return username;

        try {
            const result = await dispatch(validateToken()).unwrap();
            return result.username || null;
        } catch (error) {
            return null;
        }
    };

  return getUsername;
};

export function transformPayload<T extends object>(obj: T): T {
    return Object.fromEntries(
        Object.entries(obj).map(([key, value]) => 
            [key, value === '' ? null : value]
        )
    ) as T;
}