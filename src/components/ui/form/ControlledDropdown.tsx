import { getSelectedOption } from "@/lib/utils/appFunctions";
import { Controller, FieldValues } from "react-hook-form";
import { FormDropdown } from "./FormDropdown";
import { ControlledDropdownProps, Option } from "./types";

export const ControlledDropdown = <T extends FieldValues>({
    name,
    control,
    label,
    options,
    isMulti = false,
}: ControlledDropdownProps<T>) => (
    <Controller
        name={name}
        control={control}
        render={({ field, fieldState }) => {
            const selectedValue = getSelectedOption(options, field.value);

            return (
                <FormDropdown
                    label={label}
                    options={options}
                    value={selectedValue}
                    onChange={(option) => {
                        if (isMulti) {
                            const selectedIds = (option as Option[])?.map(opt => opt.value) ?? [];
                            field.onChange(selectedIds);
                        } else {
                            field.onChange((option as Option)?.value ?? null);
                        }
                    }}
                    onBlur={field.onBlur}
                    error={fieldState.error}
                    isLoading={options.length === 0}
                    isMulti={isMulti}
                />
            );
        }}
    />
);