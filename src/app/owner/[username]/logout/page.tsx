'use client';

import { logout } from "@/lib/apis/account/logout";
import { useAppDispatch } from "@/lib/store/hooks";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function Logout() {

    const dispatch = useAppDispatch();
    const router = useRouter();

    useEffect(() => {
        dispatch(logout());
        router.push('/');
    }, []);

    return (
        <div className="flex items-center justify-center">
            <h1>Logging out........!</h1>
        </div>
    );
}
