
import React, { CSSProperties, ReactElement } from "react";

import {
    Button,
    ButtonProps,
    ButtonSlots,
} from "@fluentui/react-components";

interface IProps {
    text?: string;
    appearence?: ButtonProps["appearance"];
    icon?: ButtonSlots["icon"];
    /**
     * 'small' | 'medium' | 'large'
     */
    size?: ButtonProps["size"];
    shape?: ButtonProps["shape"];
    className?: string;
    style?: CSSProperties;
    onClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => Promise<void> | void
};

const CustomButton: React.FunctionComponent<IProps> = ({
    text,
    appearence,
    icon,
    size = "medium",
    shape = "square",
    className = "",
    style,
    onClick,
}): ReactElement => {
    return (
        <Button
            appearance={appearence}
            icon={icon}
            size={size}
            shape={shape}
            className={className}
            style={style}
            onClick={(e) => {
                if(onClick) onClick(e);
            }}
        >
            {text}
        </Button>
    );
};

export default CustomButton;