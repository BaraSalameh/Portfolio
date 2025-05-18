'use client';

import { InputHTMLAttributes } from 'react';
import { FieldError, UseFormRegisterReturn } from 'react-hook-form';
import { Paragraph } from './Paragraph';

interface FormInputProps extends InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    registration?: UseFormRegisterReturn;
    error?: FieldError;
}

export const FormInput = ({
    label,
    registration,
    error,
    ...rest
}: FormInputProps) => {

    const inputClasses = `
        w-full
        px-4
        py-2
        mt-1
        border
        rounded-lg
        focus:outline-none
        focus:ring-2
        ${error ? 'border-red-500 focus:ring-red-500' : 'border-gray-500 focus:ring-gray-500'}
        ${rest.className}
    `;

    return (
        <div className="space-y-1">
            {label &&
                <label className="block text-sm font-medium text-white-700">
                    <Paragraph>{label}</Paragraph>
                </label>
            }
            {rest.type === 'textarea' ? (
                <textarea
                    {...(registration as any)} // casting because it can be for input or textarea
                    {...rest}
                    className={`${inputClasses} overflow-auto scrollbar-hide`}
                />
            ) : (
                <input
                    {...registration}
                    {...rest}
                    className={inputClasses}
                />
            )}
            {error && <Paragraph intent="danger" size="sm">{error.message}</Paragraph>}
        </div>
    );
};
