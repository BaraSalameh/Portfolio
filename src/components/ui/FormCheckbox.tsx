'use client';

import { InputHTMLAttributes } from 'react';
import { FieldError, UseFormRegisterReturn } from 'react-hook-form';
import { Paragraph } from './Paragraph';

interface FormCheckboxProps extends InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    registration?: UseFormRegisterReturn;
    error?: FieldError;
}

export const FormCheckbox = ({
    label,
    registration,
    error,
    ...rest
}: FormCheckboxProps) => {
    return (
        <div className="flex items-center space-x-2">
            <input
                type="checkbox"
                {...registration}
                {...rest}
                className={`h-5 w-4 accent-green-700 cursor-pointer`}
            />
            {label && (
                <Paragraph size="sm">{label}</Paragraph>
            )}
            {error && <Paragraph intent="danger" size="sm">{error.message}</Paragraph>}
        </div>
    );
};
