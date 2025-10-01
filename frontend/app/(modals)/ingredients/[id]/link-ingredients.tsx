import { View, Text, ScrollView, TextInput, Alert } from "react-native";
import { Button } from "@/components/ui/button";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useState, useEffect } from "react";
import { GoodService, IngredientMappingService } from "@/src/api-client";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

type ModalStep = 'search' | 'create-good' | 'select-price' | 'add-price';

interface Good {
  id: number;
  friendlyName: string;
}

interface GoodTransaction {
  id: number;
  price: number;
  unit: string;
  createdDate: string;
}

interface PendingAction {
  type: 'create-good' | 'add-price';
  data: any;
}

// This is a simple modal for linking an ingredient to a product or entering a temporary cost
export default function LinkIngredientModal() {
  const { id: ingredientId } = useLocalSearchParams();
  const router = useRouter();
  const queryClient = useQueryClient();
  
  // State management
  const [currentStep, setCurrentStep] = useState<ModalStep>('search');
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedGood, setSelectedGood] = useState<Good | null>(null);
  const [newGoodName, setNewGoodName] = useState('');
  const [newPrice, setNewPrice] = useState('');
  const [newUnit, setNewUnit] = useState('');
  const [pendingActions, setPendingActions] = useState<PendingAction[]>([]);

  // Queries
  const goodsQuery = useQuery({
    queryKey: ['goods', searchQuery],
    queryFn: () => GoodService.getGoodSearch(searchQuery)
  });

  const latestPriceQuery = useQuery({
    queryKey: ['good-price', selectedGood?.id],
    queryFn: () => selectedGood ? GoodService.getGoodLatestPrice(selectedGood.id) : null,
    enabled: !!selectedGood && currentStep === 'select-price'
  });

  // Mutations
  const createGoodMutation = useMutation({
    mutationFn: async (goodName: string) => {
      return GoodService.postGood({ friendlyName: goodName });
    }
  });

  const addPriceMutation = useMutation({
    mutationFn: async ({ goodId, price, unit }: { goodId: number, price: number, unit: string }) => {
      return GoodService.postGoodPrice(goodId, { price, unit });
    }
  });

  const linkMutation = useMutation({
    mutationFn: async (goodId: number) => {
      return IngredientMappingService.postApiIngredientMapping({
        ingredientId: Number(ingredientId),
        goodId: goodId,
        userId: 1
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['ingredient-mapping'] });
      queryClient.invalidateQueries({ queryKey: ['recipe'] });
      router.back();
    }
  });

  // Step handlers
  const handleGoodClick = (good: Good) => {
    setSelectedGood(good);
    setCurrentStep('select-price');
  };

  const handleCreateGood = () => {
    setCurrentStep('create-good');
  };

  const handleCreateGoodSubmit = () => {
    if (!newGoodName.trim()) {
      Alert.alert('Error', 'Please enter a good name');
      return;
    }
    
    setPendingActions(prev => [...prev, {
      type: 'create-good',
      data: { name: newGoodName }
    }]);
    
    // Simulate the good being created for UI purposes
    const tempGood: Good = {
      id: -1, // Temporary ID
      friendlyName: newGoodName
    };
    
    setSelectedGood(tempGood);
    setNewGoodName('');
    setCurrentStep('add-price');
  };

  const handleAddPrice = () => {
    setCurrentStep('add-price');
  };

  const handleAddPriceSubmit = () => {
    if (!newPrice || !newUnit) {
      Alert.alert('Error', 'Please enter both price and unit');
      return;
    }

    setPendingActions(prev => [...prev, {
      type: 'add-price',
      data: { 
        goodId: selectedGood?.id,
        price: parseFloat(newPrice),
        unit: newUnit
      }
    }]);

    setNewPrice('');
    setNewUnit('');
    setCurrentStep('select-price');
  };

  const handleFinalSubmit = async () => {
    try {
      let finalGoodId = selectedGood?.id;

      // Execute pending actions in order
      for (const action of pendingActions) {
        if (action.type === 'create-good') {
          const result = await createGoodMutation.mutateAsync(action.data.name);
          finalGoodId = result.id;
        } else if (action.type === 'add-price') {
          await addPriceMutation.mutateAsync({
            goodId: finalGoodId || action.data.goodId,
            price: action.data.price,
            unit: action.data.unit
          });
        }
      }

      // Finally create the ingredient mapping
      if (finalGoodId) {
        await linkMutation.mutateAsync(finalGoodId);
      }
    } catch (error) {
      Alert.alert('Error', 'Failed to complete the linking process');
    }
  };

  const goBack = () => {
    if (currentStep === 'search') {
      router.back();
    } else if (currentStep === 'create-good') {
      setCurrentStep('search');
    } else if (currentStep === 'select-price') {
      setCurrentStep('search');
      setSelectedGood(null);
    } else if (currentStep === 'add-price') {
      setCurrentStep('select-price');
    }
  };

  return (
    <View className="flex-1 bg-white p-6">
      {/* Header */}
      <View className="flex-row items-center justify-between mb-4">
        <Button variant="outline" onPress={goBack}>
          Back
        </Button>
        <Text className="text-xl font-bold">Link Ingredient</Text>
        <Button variant="outline" onPress={() => router.back()}>
          Cancel
        </Button>
      </View>

      {/* Search Step */}
      {currentStep === 'search' && (
        <>
          <TextInput
            className="border border-gray-300 rounded-lg p-3 mb-4"
            placeholder="Search for goods..."
            value={searchQuery}
            onChangeText={setSearchQuery}
          />
          
          <Button 
            className="mb-4"
            onPress={handleCreateGood}
          >
            + Add Generic Good
          </Button>

          <ScrollView className="flex-1">
            {goodsQuery.data?.map((good: Good) => (
              <Button
                key={good.id}
                variant="outline"
                className="mb-2 justify-start"
                onPress={() => handleGoodClick(good)}
              >
                {good.friendlyName}
              </Button>
            ))}
          </ScrollView>
        </>
      )}

      {/* Create Good Step */}
      {currentStep === 'create-good' && (
        <>
          <Text className="text-lg mb-4">Create New Good</Text>
          <TextInput
            className="border border-gray-300 rounded-lg p-3 mb-4"
            placeholder="Enter good name..."
            value={newGoodName}
            onChangeText={setNewGoodName}
          />
          <Button onPress={handleCreateGoodSubmit}>
            Create Good
          </Button>
        </>
      )}

      {/* Select Price Step */}
      {currentStep === 'select-price' && selectedGood && (
        <>
          <Text className="text-lg mb-4">Price for {selectedGood.friendlyName}</Text>
          
          {latestPriceQuery.data && (
            <View className="bg-gray-100 p-4 rounded-lg mb-4">
              <Text className="font-semibold">Latest Price</Text>
              <Text>${latestPriceQuery.data.price} per {latestPriceQuery.data.unit}</Text>
            </View>
          )}

          <Button 
            variant="outline" 
            className="mb-4"
            onPress={handleAddPrice}
          >
            Enter New Price
          </Button>

          <Button onPress={handleFinalSubmit}>
            Link This Good
          </Button>
        </>
      )}

      {/* Add Price Step */}
      {currentStep === 'add-price' && (
        <>
          <Text className="text-lg mb-4">Add Price for {selectedGood?.friendlyName}</Text>
          
          <TextInput
            className="border border-gray-300 rounded-lg p-3 mb-4"
            placeholder="Price (e.g., 3.99)"
            value={newPrice}
            onChangeText={setNewPrice}
            keyboardType="numeric"
          />
          
          <TextInput
            className="border border-gray-300 rounded-lg p-3 mb-4"
            placeholder="Unit (e.g., lb, each, oz)"
            value={newUnit}
            onChangeText={setNewUnit}
          />
          
          <Button onPress={handleAddPriceSubmit}>
            Add Price
          </Button>
        </>
      )}

      {/* Pending Actions Summary */}
      {pendingActions.length > 0 && (
        <View className="bg-blue-50 p-3 rounded-lg mt-4">
          <Text className="font-semibold text-blue-800 mb-2">Pending Actions:</Text>
          {pendingActions.map((action, idx) => (
            <Text key={idx} className="text-blue-600 text-sm">
              • {action.type === 'create-good' ? `Create "${action.data.name}"` : 
                   `Add price $${action.data.price}/${action.data.unit}`}
            </Text>
          ))}
        </View>
      )}
    </View>
  );
}
