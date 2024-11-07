class Solution:
    def reverse(self, nums, i, j):
        while i < j:
            if 0 <= i < len(nums) and 0 <= j < len(nums):
                temp = nums[i]
                nums[i] = nums[j]
                nums[j] = temp
            i += 1
            j -= 1
        return nums
    def rotate(self, nums: List[int], k: int) -> None:
        """
        Do not return anything, modify nums in-place instead.
        """
        k = k % (len(nums))
        i = 0
        j = len(nums) - k - 1
        nums = self.reverse(nums, i, j)
        i = len(nums) - k
        j = len(nums) - 1
        nums = self.reverse(nums, i, j)
        nums = self.reverse(nums, 0, len(nums)-1)

        


        