import * as React from "react";
import { View } from "react-native";

import { cn } from "./utils";

function Separator({
  className,
  orientation = "horizontal",
  decorative = true,
  ...props
}: React.ComponentProps<typeof View> & {
  orientation?: "horizontal" | "vertical";
  decorative?: boolean;
}) {
  return (
    <View
      role={decorative ? "none" : "separator"}
      className={cn(
        "bg-border shrink-0",
        orientation === "horizontal"
          ? "h-px w-full"
          : "h-full w-px",
        className,
      )}
      {...props}
    />
  );
}

export { Separator };