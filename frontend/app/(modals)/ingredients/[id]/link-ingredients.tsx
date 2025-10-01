import { View, Text, ScrollView } from "react-native";
import { Button } from "@/components/ui/button";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useState } from "react";
import { GoodService, IngredientMappingService } from "@/src/api-client";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

// This is a simple modal for linking an ingredient to a product or entering a temporary cost
export default function LinkIngredientModal() {
  const { id: ingredientId } = useLocalSearchParams();
  const router = useRouter();
  const queryClient = useQueryClient();
  const [selectedGoodId, setSelectedGoodId] = useState<number | null>(null);

  // Fetch available goods to link to
  const goodsQuery = useQuery({
    queryKey: ['goods'],
    queryFn: () => GoodService.getGoodSearch('')
  });

  // Check if ingredient is already linked
  const existingMappingQuery = useQuery({
    queryKey: ['ingredient-mapping', ingredientId],
    queryFn: () => IngredientMappingService.getApiIngredientMapping(Number(ingredientId))
  });

  // Create the mapping
  const linkMutation = useMutation({
    mutationFn: async (goodId: number) => {
      return IngredientMappingService.postApiIngredientMapping({
        ingredientId: Number(ingredientId),
        goodId: goodId,
        userId: 1 // Hard-coded for now, as in your backend
      });
    },
    onSuccess: () => {
      // Invalidate queries that depend on ingredient mappings
      queryClient.invalidateQueries({ queryKey: ['ingredient-mapping'] });
      queryClient.invalidateQueries({ queryKey: ['recipe'] });
      router.back();
    }
  });

  const handleLink = () => {
    if (selectedGoodId) {
      linkMutation.mutate(selectedGoodId);
    }
  };

  return (
    <View className="flex-1 bg-white p-6">
      <Text className="text-xl font-bold mb-4">Link Ingredient to Product</Text>
      
      {existingMappingQuery.data && (
        <Text className="mb-4 text-green-600">
          Already linked to: {existingMappingQuery.data.friendlyName}
        </Text>
      )}

      <ScrollView className="flex-1">
        {goodsQuery.data?.map((good: any) => (
          <Button
            key={good.id}
            variant={selectedGoodId === good.id ? "default" : "outline"}
            className="mb-2"
            onPress={() => setSelectedGoodId(good.id)}
          >
            {good.friendlyName}
          </Button>
        ))}
      </ScrollView>

      <View className="flex-row gap-2 mt-4">
        <Button variant="outline" className="flex-1" onPress={() => router.back()}>
          Cancel
        </Button>
        <Button 
          className="flex-1" 
          onPress={handleLink}
          disabled={!selectedGoodId || linkMutation.isPending}
        >
          {linkMutation.isPending ? 'Linking...' : 'Link Product'}
        </Button>
      </View>
    </View>
  );
}
