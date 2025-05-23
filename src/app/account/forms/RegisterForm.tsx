'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/Button";
import Image from "next/image";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormInput } from "@/components/ui/FormInput";
import { register as registerAPI } from "@/lib/apis/account/register";
import { List } from "@/components/ui/List";
import { FormCheckbox } from "@/components/ui/FormCheckbox";
import { RegisterFormData, registerSchema } from "@/lib/schemas/registerSchema";

const RegisterForm = () => {

    const dispatch = useAppDispatch();
    const { loading, error } = useAppSelector((state) => state.auth);
    
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<RegisterFormData>({
        resolver: zodResolver(registerSchema),
        defaultValues:{
            rememberMe: false
        }
    });
    
    const onSubmit = (data: RegisterFormData) => {
        dispatch(registerAPI(data));
    };
      
    return(
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <Paragraph size="xl" className="py-3">
                Register
            </Paragraph>
            <FormInput
                label="Firstname"
                type="text"
                placeholder="Ex. John"
                registration={register('firstname')}
                error={errors.firstname}
            />
            <FormInput
                label="Lastname"
                type="text"
                placeholder="Ex. Doe"
                registration={register('lastname')}
                error={errors.lastname}
            />
            <FormInput
                label="Email"
                type="text"
                placeholder="Enter your email"
                registration={register('email')}
                error={errors.email}
            />
            <FormInput
                label="Confirm Email"
                type="text"
                placeholder="Re-enter your email"
                registration={register('reEmail')}
                error={errors.reEmail}
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
                {loading ? 'Registering...' : 'Register'}
            </Button>
        </form>
    );
};

export default RegisterForm;