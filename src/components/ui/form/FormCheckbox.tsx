'use client';

import { Paragraph } from '../Paragraph';
import { FormCheckboxProps } from './types';

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
