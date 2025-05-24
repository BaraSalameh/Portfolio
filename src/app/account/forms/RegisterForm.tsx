'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { register } from "@/lib/apis/account/register";
import { RegisterFormData, registerSchema } from "@/lib/schemas";
import { ControlledForm } from "@/components/ui/form";

const RegisterForm = () => {

    const dispatch = useAppDispatch();
    const { loading, error } = useAppSelector((state) => state.auth);

    const onSubmit = (data: RegisterFormData) => {
        dispatch(register(data));
    };

    return(
        <ControlledForm
            schema={registerSchema}
            onSubmit={onSubmit}
            items={[
                {as: 'Input', name: 'firstname', label: 'Firstname', placeholder: 'John'},
                {as: 'Input', name: 'lastname', label: 'Lastname', placeholder: 'Doe'},
                {as: 'Input', name: 'email', label: 'Email', placeholder: 'john.doe@example.com', type: 'Email'},
                {as: 'Input', name: 'reEmail', label: 'Confirm Email', placeholder: 'Re-enter your email', type: 'Email'},
                {as: 'Input', name: 'password', label: 'Password', placeholder: '* * * * * * * *', type: 'Password'},
                {as: 'Checkbox', name: 'rememberMe', label: 'Remember me'},
            ]}
            error={error}
            loading={loading}
            defaultValues={{rememberMe: false}}
        >
            <Paragraph size="xl" className="py-3">
                Register
            </Paragraph>
        </ControlledForm>
    );
};

export default RegisterForm;