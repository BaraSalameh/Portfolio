import { useFieldArray, Controller } from "react-hook-form";
import { ControlledDropdown } from "./ControlledDropdown";
import { Button } from "./Button";
import { ResponsiveIcon } from "../ResponsiveIcon";
import { Trash2 } from "lucide-react";
import { Header, Main } from "@/components/shared";

export const FieldArray = ({
    name,
    label,
    control,
    fields: fieldConfigs
}: any) => {

    const { fields, append, remove } = useFieldArray({
        control,
        name,
    });

    return (
        <div className="space-y-2">
            <h3 className="text-white text-sm font-medium">{label}</h3>
            <div className="space-y-3">
                {fields.map((field, i) => (
                    <div key={field.id} className="border p-5 rounded-2xl space-y-3">
                        <Header paddingX='none' paddingY='none' itemsX='end'>
                            <ResponsiveIcon icon={Trash2} onClick={() => remove(i)} className="cursor-pointer" />
                        </Header>
                        {fieldConfigs.map((config: any, j: number) => (
                            <ControlledDropdown
                                key={j}
                                name={`${name}[${i}].${config.name}`}
                                control={control}
                                label={config.label}
                                options={config.options || []}
                            />
                        ))}
                    </div>
                ))}
            </div>
            <Button
                type="button"
                onClick={() => append(
                    fieldConfigs.reduce((acc: any, config: any) => {
                        acc[config.name] = '';
                        return acc;
                    }, {})
                )}
            >
                <ResponsiveIcon />
                Add new
            </Button>
        </div>
    );
};
