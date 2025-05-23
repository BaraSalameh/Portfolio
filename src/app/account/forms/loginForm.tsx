'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/form/Button";
import Image from "next/image";
import { useForm } from "react-hook-form";
import { LoginFormData, loginSchema } from "@/lib/schemas/loginSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { login } from "@/lib/apis/account/login";
import { List } from "@/components/ui/List";
import { FormCheckbox, FormInput } from "@/components/ui/form";

const LoginForm = () => {

    const dispatch = useAppDispatch();
    const { loading, error } = useAppSelector((state) => state.auth);
    
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginFormData>({
        resolver: zodResolver(loginSchema),
        defaultValues:{
            rememberMe: false
        }
    });
    
    const onSubmit = (data: LoginFormData) => {
        dispatch(login(data));
    };

    return(
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <Paragraph size="xl" className="py-3">
                Login
            </Paragraph>
            <FormInput
                label="Email"
                type="text"
                placeholder="Enter your email"
                registration={register('email')}
                error={errors.email}
            />

            <FormInput
                label="Password"
                type="password"
                placeholder="Enter your password"
                registration={register('password')}
                error={errors.password}
            />

            <FormCheckbox
                label="Remember me"
                registration={register('rememberMe')}
                error={errors.rememberMe}
            />

            {Array.isArray(error) && error.length > 1 ? (
                <List intent="danger" size="sm">
                    {error.map((e: string, i: number) => (
                        <li key={i}>{e}</li>
                    ))}
                </List>
            ) : (
                error && <Paragraph intent="danger" size="sm">{error}</Paragraph>
            )}

            <Button intent="standard" rounded="full" size="lg" type="submit" disabled={loading}>
                <Image
                    className="dark:invert"
                    src="/vercel.svg"
                    alt="Vercel logomark"
                    width={20}
                    height={20}
                />
                {loading ? 'Logging in...' : 'Login'}
            </Button>
        </form>
    );
};

export default LoginForm;