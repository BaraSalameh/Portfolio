'use client';

import Select from 'react-select';
import { FieldError, UseFormRegisterReturn } from 'react-hook-form';
import { Paragraph } from './Paragraph';

interface Option {
    label: string;
    value: string;
}

interface FormDropdownProps {
    label?: string;
    options: Option[];
    value?: Option;
    onChange?: (option: Option | null) => void;
    error?: FieldError;
    isSearchable?: boolean;
    isClearable?: boolean;
    placeholder?: string;
}

export const FormDropdown = ({
    label,
    options,
    value,
    onChange,
    error,
    isSearchable = true,
    isClearable = true,
    placeholder = 'Select...',
}: FormDropdownProps) => {
    const customStyles = {
        control: (provided: any, state: any) => ({
            ...provided,
            padding: '0.25rem 0.5rem',
            borderRadius: '0.5rem',
            borderColor: error ? '#f87171' : '#6b7280', // red-500 or gray-500
            boxShadow: state.isFocused
                ? `0 0 0 2px ${error ? '#f87171' : '#6b7280'}50`
                : undefined,
            '&:hover': {
                borderColor: error ? '#f87171' : '#6b7280',
            },
        }),
        menu: (provided: any) => ({
            ...provided,
            zIndex: 10,
        }),
    };

    return (
        <div className="space-y-1">
            {label && (
                <label className="block text-sm font-medium text-white-700">
                <Paragraph>{label}</Paragraph>
                </label>
            )}
            <Select
                options={options}
                value={value}
                onChange={onChange}
                styles={customStyles}
                isSearchable={isSearchable}
                isClearable={isClearable}
                placeholder={placeholder}
            />
            {error && <Paragraph intent="danger" size="sm">{error.message}</Paragraph>}
        </div>
    );
};
